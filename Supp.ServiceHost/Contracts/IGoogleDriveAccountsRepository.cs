using Supp.ServiceHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
{
    public interface IGoogleDriveAccountsRepository
    {
        Task<GoogleDriveAccountResult> GetAllGoogleDriveAccounts();

        Task<GoogleDriveAccountResult> GetGoogleDriveAccountsById(long id);

        Task<GoogleDriveAccountResult> UpdateGoogleDriveAccount(GoogleDriveAccountDto dto);

        Task<GoogleDriveAccountResult> AddGoogleDriveAccount(GoogleDriveAccountDto dto);

        Task<GoogleDriveAccountResult> DeleteGoogleDriveAccountById(long id);

        Task<GoogleDriveAccountResult> AddGoogleDriveCredentials(string parametersJsonString);
    }
}
