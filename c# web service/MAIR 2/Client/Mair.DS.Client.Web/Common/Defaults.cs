using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DS.Client.Web.Common
{
    public class Defaults
    {
        public const string BaseUrl = "http://localhost:5000";

        public static string ClientUrl = "";

        public const string MairDSClientWebAccessTokenCookieName = "MairDSClientWebAccessTokenCookieName";

        public const string MairDSClientWebAuthenticatedUserNameCookieName = "MairDSClientWebAuthenticatedUserNameCookieName";

        public const string MairDSClientWebAuthenticatedUserIdCookieName = "MairDSClientWebAuthenticatedUserIdCookieName";

        public const string MairDSClientWebAuthenticatedUserRolesCookieName = "MairDSClientWebAuthenticatedUserRolesCookieName";

        public const int CookieExpireInSeconds = 2592000;

        public const string DefaultPassword = "Password!123";

        public class Roles
        {
            public const string AdminRole = "Admin";
            public const string SuperUserRole = "SuperAdmin";
            public const string UserRole = "User";
            public const string GuestRole = "Guest";
        }
    }
}
