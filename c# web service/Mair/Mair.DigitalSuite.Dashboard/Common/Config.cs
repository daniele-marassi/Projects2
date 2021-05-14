using System;
using System.Collections.Generic;

namespace Mair.DigitalSuite.Dashboard.Common
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
                public static string BaseUrl { get; set; }
                public static int PageSize { get; set; }
            }

            public class Constants
            {
                public const string DefaultPassword = "Password!123";
                public const string MairDigitalSuiteAccessTokenCookieName = "MairDigitalSuiteAccessToken";
                public const string MairDigitalSuiteAuthenticatedUserCookieName = "MairDigitalSuiteAuthenticatedUser";
                public const string MairDigitalSuiteErrorsCookieName = "MairDigitalSuiteErrors";
            }
        }
    }
}