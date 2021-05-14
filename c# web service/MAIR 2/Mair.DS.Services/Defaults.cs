using Microsoft.Extensions.Configuration;

namespace Mair.DS.Services
{
    /// <summary>
    /// 
    /// </summary>
    public static class Defaults
    {
        #region Fields and Properties

        private const string ApiRoot = "api";
        private const string HubRoot = "hub";

        /// <summary>
        /// Gets the name of the tag route.
        /// </summary>
        /// <value>
        /// The name of the tag route.
        /// </value>
        public const string TagRouteName = @"api/tags";

        public const string SimulatedConnectorRouteName = @"api/simulatedconnector";

        public const string NodesRouteName = @"api/nodes";

        public const string AuthenticationsRouteName = @"api/authentications";

        public const string RolesRouteName = @"api/roles";

        public const string UsersRouteName = @"api/users";

        public const string UserRolesRouteName = @"api/userRoles";

        public const string RolePathsRouteName = @"api/rolePaths";

        public const string TagDispatcherName = @"api/tagdispatcher";

        public const string EventActionName = @"api/eventaction";

        public const string MenuItemsRouteName = @"api/menuItems";

        public const string PlcDataHub = @"hub/plcData";

        public static string Token { get; set; }

        public static IConfigurationSection JwtAppSettingOptions { get; set; }

        #endregion

        #region Constructors

        static Defaults()
        {

        }

        #endregion

        #region Methods
        #endregion

        public class Roles
        { 
            public const string RoleAdmin = "Admin";

            public const string RoleUser = "User";

            public const string RoleSuperUser = "SuperUser";

            public const string RoleGuest = "Guest";
        }
    }
}
