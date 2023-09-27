using System;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Supp.Site.Common.Config;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Supp.Models;
using Additional.NLog;
using System.Reflection;
using NLog;
using System.Configuration;
using Supp.Site.Repositories;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace Supp.Site
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
            if (Program.TokensArchive == null || Program._webSpeechResultList == null)
            {
                using (var logger = new NLogScope(_classLogger, _nLogUtility.GetMethodToNLog(MethodInfo.GetCurrentMethod())))
                {
                    if (Program.TokensArchive == null) logger.Error("Startup***************" + " TokensArchive == null");
                    if (Program._webSpeechResultList == null) logger.Error("Startup***************" + " _webSpeechResultList == null");
                }

                GeneralSettings.SetGeneralSettings(configuration);
                var ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString();
                GeneralSettings.Static.BaseUrl = GeneralSettings.Static.BaseUrl.Replace("IP", ip);

                if (Program.TokensArchive == null) Program.TokensArchive = new ConcurrentDictionary<long, TokenDto>();

                if (Program._webSpeechResultList == null)
                {
                    try
                    {
                        Program._webSpeechResultList = new ConcurrentDictionary<long, WebSpeechResult>();

                        var accessTokenAdminForService = configuration.GetSection("AppSettings:AccessTokenAdminForService").Value;

                        var userRepo = new UsersRepository();

                        var getAllUsersResult = userRepo.GetAllUsers(accessTokenAdminForService).GetAwaiter().GetResult();

                        if (getAllUsersResult.Successful)
                        {
                            var userIdList = getAllUsersResult.Data.Select(_ => _.Id).ToList();

                            var webSpeachesRepo = new WebSpeechesRepository();

                            var getAllWebSpeechesResult = webSpeachesRepo.GetAllWebSpeeches(accessTokenAdminForService).GetAwaiter().GetResult();

                            if (getAllWebSpeechesResult.Successful)
                            {
                                foreach (var userId in userIdList)
                                {
                                    Program._webSpeechResultList[userId] = getAllWebSpeechesResult;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            services.AddMvc();
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.AccessDeniedPath = "/you-are-not-allowed-page";
                    options.LoginPath = "/Home/Login";
                }
            );

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
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}