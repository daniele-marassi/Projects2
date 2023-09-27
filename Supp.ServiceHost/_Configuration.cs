using Microsoft.Extensions.Configuration;
using System;

namespace Supp.ServiceHost
{
    public static class _Configuration
    {
        public static IConfigurationRoot BuildConfigurationRoot()
        {

            var args = Environment.GetCommandLineArgs();
            var envArg = Array.IndexOf(args, "--environment");
            var envFromArgs = envArg >= 0 ? args[envArg + 1] : null;

            var aspnetcore = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var dotnetcore = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            var environment = envFromArgs ?? (string.IsNullOrWhiteSpace(aspnetcore)
                ? dotnetcore
                : aspnetcore);

            var configuration = new ConfigurationBuilder()
                .AddCommandLine(Environment.GetCommandLineArgs())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{environment}.json",
                    optional: true)
                .Build();

            return configuration;
        }
    }
}
