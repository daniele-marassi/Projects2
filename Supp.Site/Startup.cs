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

namespace Supp.Site
{
    public class Startup
    {
        private IConfiguration _configuration { get; }

        public static ConcurrentDictionary<long, WebSpeechResult> _webSpeechResultList;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _webSpeechResultList = new ConcurrentDictionary<long, WebSpeechResult>();
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

            GeneralSettings.SetGeneralSettings(_configuration);
            var ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString();
            GeneralSettings.Static.BaseUrl = GeneralSettings.Static.BaseUrl.Replace("IP",ip);
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