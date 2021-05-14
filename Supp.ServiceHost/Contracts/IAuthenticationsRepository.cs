using Supp.ServiceHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
{
    public interface IAuthenticationsRepository
    {
        Task<AuthenticationResult> GetAllAuthentications();

        Task<AuthenticationResult> GetAuthenticationsById(long id);

        Task<AuthenticationResult> GetAuthenticationsByUserName(string userName);

        Task<AuthenticationResult> DisableAuthenticationsByUserName(string userName);

        Task<AuthenticationResult> UpdateAuthentication(AuthenticationDto dto);

        Task<AuthenticationResult> AddAuthentication(AuthenticationDto dto);

        Task<AuthenticationResult> DeleteAuthenticationById(long id);
    }
}
