using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supp.ServiceHost.Contexts;
using Supp.ServiceHost.Contracts;
using Supp.ServiceHost.Repositories;
using Supp.ServiceHost.Services.Token;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using static Supp.ServiceHost.Common.Config;
using System.Net;
using Additional.NLog;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using Supp.Models;
using Microsoft.AspNetCore.SignalR;
using GoogleManagerModels;
using Newtonsoft.Json;
using NLog;
using System.Reflection;
using System.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace Supp.ServiceHost
{
    public class Startup
    {
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();

        private static IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            Init(configuration, classLogger, nLogUtility);
        }

        //private async Task Service(IConfiguration configuration, Logger _classLogger, NLogUtility _nLogUtility)
        //{
        //    while (true)
        //    {
        //        Init(configuration, _classLogger, _nLogUtility);
        //        System.Threading.Thread.Sleep(10000);
        //    }
        //}

        private void Init(IConfiguration configuration, Logger _classLogger, NLogUtility _nLogUtility)
        {
            if (Program.TokensArchive == null)
            {
                using (var logger = new NLogScope(_classLogger, _nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                {
                    if(Program.TokensArchive == null) logger.Error("Startup***************" + " TokensArchive == null");
                }

                GeneralSettings.Static.SuppDatabaseConnection = _configuration.GetConnectionString("SuppDatabaseConnection");
                GeneralSettings.Static.LimitLogFileInMB = Int32.Parse(_configuration.GetSection("AppSettings:LimitLogFileInMB").Value);
                GeneralSettings.Static.ExpireDays = double.Parse(_configuration.GetSection("AppSettings:ExpireDays").Value);

                Program.TokensArchive = new ConcurrentDictionary<long, TokenDto>();

                SuppDatabaseContext _context;

                var optionsBuilder = new DbContextOptionsBuilder<SuppDatabaseContext>();

                try
                {
                    optionsBuilder.UseSqlServer(GeneralSettings.Static.SuppDatabaseConnection, builder =>
                    {
                        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    });

                    _context = new SuppDatabaseContext(optionsBuilder.Options);

                    var iTokensRepo = new TokensRepository(_context);

                    var getAllTokensResult = iTokensRepo.GetAllTokens().GetAwaiter().GetResult();

                    if (getAllTokensResult != null && getAllTokensResult.Successful && getAllTokensResult.Data.Count > 0)
                    {
                        foreach (var tokenDto in getAllTokensResult.Data)
                        {
                            tokenDto.Roles = JsonConvert.DeserializeObject<List<string>>(tokenDto.RolesInJson);

                            Program.TokensArchive.TryAdd(tokenDto.UserId, tokenDto);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var nLogUtility = new NLogUtility();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc();
            services.AddCors();

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
            services.AddScoped<IGoogleAccountsRepository, GoogleAccountsRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<IMediaConfigurationsRepository, MediaConfigurationsRepository>();
            services.AddScoped<IGoogleAuthsRepository, GoogleAuthsRepository>();
            services.AddScoped<IWebSpeechesRepository, WebSpeechesRepository>();
            services.AddScoped<IExecutionQueuesRepository, ExecutionQueuesRepository>();
            services.AddScoped<ISongsRepository, SongsRepository>();
            services.AddScoped<ITokensRepository, TokensRepository>();

            //Task.Run(() => Service(_configuration, classLogger, nLogUtility));
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
