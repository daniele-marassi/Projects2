using Microsoft.AspNetCore.Mvc;
using System;

namespace Mair.DS.Common
{
    public class BaseService<T, K> : Controller
        where T : class, new()
    {
        public K Engine { get; set; }
        public IServiceProvider Provider { get; set; }

        public BaseService(IServiceProvider provider)
        {
            Provider = provider;
        }
    }
}
