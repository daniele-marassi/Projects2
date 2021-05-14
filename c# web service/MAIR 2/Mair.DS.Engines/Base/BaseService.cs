using Microsoft.AspNetCore.Mvc;
using System;

namespace Mair.DS.Engines.Base
{
    public class BaseService<T, K> : Controller
        where K : BaseEngine<T>
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
