using Mair.DS.Engines.TagDispatcher;
using Mair.DS.Common.Loggers;
using Mair.DS.Data.Context;
using Mair.DS.Engines.Core.EventManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mair.DS.Services.IoC
{
    public static class MairDependencyInjection
    {
        public static IServiceCollection AddMairLogger(this IServiceCollection services)
        {
            services.AddSingleton<ILogger, ConsoleLogger>();
            return services;
        }

        public static IServiceCollection AddMairServices(this IServiceCollection services)
        {
            services.AddSingleton<ITagDispatcher, TagDispatcher>();
            services.AddSingleton<IEventManager, EventManager>();
            return services;
        }

        public static IServiceCollection AddMairContext(this IServiceCollection services)
        {
            Action<DbContextOptionsBuilder> optionBuilderAction = new Action<DbContextOptionsBuilder>(
                options =>
                options.UseSqlServer(Models.Defaults.ConnectionString)
                .UseLazyLoadingProxies()
                );

            services.AddDbContext<AutomationContext>(optionBuilderAction);
            //services.AddDbContext<AutomationContext>(optionBuilderAction);
            services.AddDbContext<EventManagerContext>(optionBuilderAction);

            return services;
        }
    }
}
