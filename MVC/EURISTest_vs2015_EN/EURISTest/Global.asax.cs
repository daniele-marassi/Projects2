using EURISTest.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EURISTest
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            // rimuovo il value provider factory di default
            var toRemove = ValueProviderFactories.Factories
              .OfType<QueryStringValueProviderFactory>()
              .Single();
            ValueProviderFactories.Factories.Remove(toRemove);

            // aggiungo il value provider custom
            var culture = CultureInfo.GetCultureInfo("it-IT");
            ValueProviderFactories.Factories.Add(
              new CultureQueryStringValueProviderFactory(culture));

            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
    }
}