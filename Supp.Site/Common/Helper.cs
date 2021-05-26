using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.Site.Common
{
    public static class Helper
    {
        public static string ToStringExtended(this String value)
        {
            if (value == null) value = "";
            return value.ToString();
        }
    }
}
