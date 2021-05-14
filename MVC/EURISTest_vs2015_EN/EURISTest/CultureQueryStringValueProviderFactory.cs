using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EURISTest
{
    internal class CultureQueryStringValueProviderFactory : ValueProviderFactory
    {
        private CultureInfo _culture;
        public CultureQueryStringValueProviderFactory(CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");

            _culture = culture;
        }

        public override IValueProvider GetValueProvider(
            ControllerContext controllerContext)
        {
            return new CultureQueryStringValueProvider(controllerContext, _culture);
        }
    }
}