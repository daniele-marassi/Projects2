using Mair.DS.Engines.TagDispatcher;
using Mair.DS.Common;
using Mair.DS.Common.Loggers;
using Mair.DS.Data.Context;
using Mair.DS.Engines.Core.EventManager;
using Mair.DS.Services.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Mair.DS.Services
{
    public class Startup
    {
        public ILogger logger;
        public ITagDispatcher tagDispatcher;
        public IEventManager eventManager;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        { 
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
                foreach (var formatter in options.InputFormatters)
                {
                    if (formatter.GetType() == typeof(SystemTextJsonInputFormatter))
                        ((SystemTextJsonInputFormatter)formatter).SupportedMediaTypes.Add(
                            Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/plain"));
                }
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            //NON CANCELLARE volutamente impostato sia in ConfigureServices che in Configure
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .WithOrigins("*")
                       .AllowAnyHeader();
            }));
            /////////

            services.AddControllers();
            
            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
                                                                        // jwt
                                                                        // get options

            Defaults.JwtAppSettingOptions = Configuration.GetSection("JwtIssuerOptions");
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Defaults.JwtAppSettingOptions["JwtIssuer"],
                        ValidAudience = Defaults.JwtAppSettingOptions["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Defaults.JwtAppSettingOptions["JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };

                    //gestione autenticazione per SignalR
                    //cfg.Events = new JwtBearerEvents
                    //{
                    //    OnMessageReceived = context =>
                    //    {
                    //        var accessToken = context.Request.Query["access_token"];

                    //        if (accessToken.Count == 0 || accessToken == String.Empty) accessToken = context.Request.Headers.Where(_ => _.Key == "access_token").FirstOrDefault().Value;
                    //        CheckCredentials checkCredentials = new CheckCredentials();
                    //        var roles = new List<string>() { Defaults.Roles.RoleAdmin, Defaults.Roles.RoleSuperUser, Defaults.Roles.RoleUser };
                    //        checkCredentials.CheckHubAuthorization(context, accessToken, Defaults.PlcDataHub, roles);
                    //        return Task.CompletedTask;
                    //    }
                    //};
                });

            //impostazione servizio SignalR
            services.AddSignalR((hubOptions) =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
            })
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            }).AddHubOptions<ConnectorHub>(options =>
            {
                options.EnableDetailedErrors = true;
            });

            // Aggiungo i servizio Logger
            services.AddMairLogger();

            // Aggiungo i servizi che mi servono (come tagdispatcher...)
            services.AddMairServices();

            //Aggiungo i diversi Contesti a database per ogni schema creato
            services.AddMairContext();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            //NON CANCELLARE volutamente impostato sia in ConfigureServices che in Configure
            app.UseCors(builder =>
            {
                builder.WithOrigins("*");
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyMethod();
            });
            /////////

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ConnectorHub>("tags/hub");
            });

            GlobalConfig();

            // Metodo per l'assegnazione del Container globale e delle istanze
            // Il Container globale serve per accere da tutta la soluzione a tutte le Istanze 
            // dei vari servizi
            GlobalAssignment(app.ApplicationServices);

            GlobalInit();
        }

        private void GlobalConfig()
        {
            Models.Defaults.ConnectionString = Configuration.GetConnectionString("MairDSConntectionString");
            Engines.Defaults.IsPlcSimulated = Configuration.GetValue<bool>("IsPlcSimulated");
        }

        private IServiceProvider GlobalAssignment(IServiceProvider serviceProvider)
        {
            Instances.serviceProvider = serviceProvider;

            logger = serviceProvider.GetService<ILogger>();
            logger.LogInfo(logger);

            logger.LogInfo("Inizio fase di assegnazione moduli");

            tagDispatcher = serviceProvider.GetService<ITagDispatcher>();
            logger.LogInfo(tagDispatcher);

            eventManager = serviceProvider.GetService<IEventManager>();
            logger.LogInfo(eventManager);

            logger.LogInfo("Finita fase di assegnazione moduli");
            return Instances.serviceProvider;
        }

        private void GlobalInit()
        {
            logger.LogInfo("Carico moduli server");
            
            logger.Init();

            tagDispatcher.Init();

            eventManager.Init();

            //tagDispatcher.Notifier += eventAction.CheckConditions;

            logger.LogInfo("Fine caricamento moduli server");
        }
    }
}
