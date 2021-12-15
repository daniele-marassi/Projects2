using Supp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
{
    public interface IGoogleAccountsRepository
    {
        Task<GoogleAccountResult> GetAllGoogleAccounts();

        Task<GoogleAccountResult> GetGoogleAccountsById(long id);

        Task<GoogleAccountResult> UpdateGoogleAccount(GoogleAccountDto dto);

        Task<GoogleAccountResult> AddGoogleAccount(GoogleAccountDto dto);

        Task<GoogleAccountResult> DeleteGoogleAccountById(long id);

        Task<GoogleAccountResult> AddGoogleCredentials(string parametersJsonString);
    }
}
