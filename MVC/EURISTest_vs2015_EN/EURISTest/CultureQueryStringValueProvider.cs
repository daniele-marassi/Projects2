using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EURISTest
{
    internal class CultureQueryStringValueProvider : NameValueCollectionValueProvider
    {
        public CultureQueryStringValueProvider(
            ControllerContext context, CultureInfo culture)
            : base(context.HttpContext.Request.QueryString, culture)
        { }
    }
}