using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supp.ServiceHost.Contexts;
using Supp.ServiceHost.Contracts;
using Supp.ServiceHost.Repositories;
using Supp.ServiceHost.Services.SignalR;
using Supp.ServiceHost.Services.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using static Supp.ServiceHost.Common.Config;
using System.Net;
using Additional.NLog;

namespace Supp.ServiceHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var nLogUtility = new NLogUtility();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc();
            services.AddCors();

            GeneralSettings.Static.SuppDatabaseConnection = Configuration.GetConnectionString("SuppDatabaseConnection");

            services.AddDbContext<SuppDatabaseContext>(_ => _.UseSqlServer(GeneralSettings.Static.SuppDatabaseConnection, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            }));

            services.AddControllersWithViews();
            services.AddScoped<IAuthenticationsRepository, AuthenticationsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUserRolesRepository, UserRolesRepository>();
            services.AddScoped<IUserRoleTypesRepository, UserRoleTypesRepository>();
            services.AddScoped<IHomeRepository, HomeRepository>();
            services.AddScoped<IGoogleDriveAccountsRepository, GoogleDriveAccountsRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<IMediaConfigurationsRepository, MediaConfigurationsRepository>();
            services.AddScoped<IGoogleDriveAuthsRepository, GoogleDriveAuthsRepository>();
            services.AddScoped<IWebSpeechesRepository, WebSpeechesRepository>();
            services.AddScoped<IExecutionQueuesRepository, ExecutionQueuesRepository>();
            services.AddScoped<ISongsRepository, SongsRepository>();

            //services.AddHostedService<HubWorker>();

            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
                                                                        // jwt
                                                                        // get options
            GeneralSettings.Static.JwtAppSettingOptions = Configuration.GetSection("JwtIssuerOptions");
            var ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString();
            GeneralSettings.Static.JwtAppSettingOptions["JwtIssuer"] = GeneralSettings.Static.JwtAppSettingOptions["JwtIssuer"].Replace("IP",ip);

            GeneralSettings.Static.LimitLogFileInMB = Int32.Parse(Configuration.GetSection("AppSettings:LimitLogFileInMB").Value);

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
                        ValidIssuer = GeneralSettings.Static.JwtAppSettingOptions["JwtIssuer"],
                        ValidAudience = GeneralSettings.Static.JwtAppSettingOptions["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GeneralSettings.Static.JwtAppSettingOptions["JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };

                    cfg.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            //if (context.Request.Path.ToString() != "/api/Authentications/GetToken" && context.Request.Path.ToString() != "/")
                            //{
                                var accessToken = context.Request.Query["access_token"];

                                nLogUtility.ClearNLogFile("mainLog", GeneralSettings.Static.LimitLogFileInMB);

                                if (accessToken.Count == 0 || accessToken == String.Empty) accessToken = context.Request.Headers.Where(_ => _.Key == "access_token").FirstOrDefault().Value;
                                CheckCredentials checkCredentials = new CheckCredentials();
                                var roles = new List<string>() { Common.Config.Roles.Constants.RoleAdmin, Common.Config.Roles.Constants.RoleSuperUser, Common.Config.Roles.Constants.RoleUser };
                                checkCredentials.CheckHubAuthorization(context, accessToken, GeneralSettings.Constants.PlcDataHub, roles);
                            //}
                            return Task.CompletedTask;
                        }
                    };
                });

            //services.AddSignalR((hubOptions) => {
            //    hubOptions.EnableDetailedErrors = true;
            //    hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
            //})
            //                .AddJsonProtocol(options => {
            //                    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            //                }).AddHubOptions<Hub>(options => {
            //                    options.EnableDetailedErrors = true;
            //                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseCors(builder =>
                builder.WithOrigins("*"));

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapHub<Hub>(GeneralSettings.Constants.PlcDataHub
                //    , options =>
                //    {
                //        options.Transports =
                //            HttpTransportType.WebSockets |
                //            HttpTransportType.LongPolling;
                //    }
                //);
            });
        }
    }
}
