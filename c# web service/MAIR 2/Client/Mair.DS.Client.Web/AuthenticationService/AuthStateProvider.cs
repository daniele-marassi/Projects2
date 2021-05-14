using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using NLog;
using Mair.DS.Client.Web.Common;
using Mair.DS.Client.Web.Models.Dto.Auth;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace Mair.DS.Client.Web.AuthenticationService
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly static Logger classLogger = LogManager.GetCurrentClassLogger();
        private readonly NLogUtility nLogUtility = new NLogUtility();

        public static bool IsAuthenticated { get; set; }
        public static bool IsAuthenticating { get; set; }

        public static string UserName { get; set; }
        public static long UserId { get; set; }
        public static List<string> Roles { get; set; }

        public static ClaimsIdentity Identity { get; set; }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (IsAuthenticating)
            {
                return null;
            }
            else if (IsAuthenticated)
            {
                Identity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("UserName", UserName),
                    new Claim("UserId", UserId.ToString()),
                    new Claim("Roles", string.Join( ",", Roles.ToArray() ) )

                }, "WebApiAuth");
            }
            else
            {
                Identity = new ClaimsIdentity();
            }

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(Identity)));
        }

        public void NotifyAuthenticationStateChanged(string userName, long userId, List<string> roles)
        {
            IsAuthenticated = true;
            UserName = userName;
            UserId = userId;
            Roles = roles;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
