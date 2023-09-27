using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Supp.Models;
using static Supp.Site.Common.Config;

namespace Supp.Site
{
    public class Program
    {
        public static ConcurrentDictionary<long, TokenDto> TokensArchive;
        public static ConcurrentDictionary<long, WebSpeechResult> _webSpeechResultList;

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
