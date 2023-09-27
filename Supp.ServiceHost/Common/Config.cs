using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Common
{
    public class Config
    {

        public class Roles
        {
            public class Constants
            {
                public const string RoleAdmin = "Admin";

                public const string RoleUser = "User";

                public const string RoleSuperUser = "SuperUser";

                public const string RoleGuest = "Guest";
            }
        }

        public class GeneralSettings
        {
            public class Static
            {
                public static string Token { get; set; }
                public static string SuppDatabaseConnection { get; set; }
                public static int LimitLogFileInMB { get; set; }
                public static double ExpireDays { get; set; }
            }

            public class Constants
            {
                public const string PlcDataHub = "/hubs/plcData";
            }
        }

    }
}
