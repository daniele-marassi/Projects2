using Mair.DS.Common.Loggers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mair.DS.Common
{
    public static class Instances
    {
        public static IServiceProvider serviceProvider { get; set; }
        private static IServiceScope serviceScope
        {
            get
            {
                if (serviceScope == null)
                {
                    serviceScope = serviceProvider.CreateScope();
                }
                return serviceScope;
            }
            set
            {
                serviceScope = value;
            }
        }
        private static ILogger logger
        {
            get
            {
                return GetInstance<ILogger>();
            }
        }

        public static T GetInstance<T>()
        {
            try
            {
                return serviceProvider.GetService<T>();
            }
            catch
            {
                try
                {
                    return GetScope<T>();
                }
                catch(Exception ex)
                {
                    logger.LogErr(ex);
                    throw ex;
                }
            }
        }

        private static T GetScope<T>()
        {
            return serviceProvider.CreateScope().ServiceProvider.GetService<T>();
        }
    }
}
