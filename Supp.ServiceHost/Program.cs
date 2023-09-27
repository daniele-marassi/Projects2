using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Supp.Models;
using Supp.ServiceHost.Contexts;
using Supp.ServiceHost.Repositories;
using System;
using System.Collections.Concurrent;
using static Supp.ServiceHost.Common.Config;
using System.Collections.Generic;
using System.Configuration;

namespace Supp.ServiceHost
{
    public class Program
    {

        public static ConcurrentDictionary<long, TokenDto> TokensArchive;

        public static void Main(string[] args)
        {
            //var _configuration = _Configuration.BuildConfigurationRoot();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
