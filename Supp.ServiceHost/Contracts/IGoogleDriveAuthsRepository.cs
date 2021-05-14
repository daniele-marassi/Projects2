using Supp.ServiceHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
{
    public interface IGoogleDriveAuthsRepository
    {
        Task<GoogleDriveAuthResult> GetAllGoogleDriveAuths();

        Task<GoogleDriveAuthResult> GetGoogleDriveAuthsById(long id);

        Task<GoogleDriveAuthResult> UpdateGoogleDriveAuth(GoogleDriveAuthDto dto);

        Task<GoogleDriveAuthResult> AddGoogleDriveAuth(GoogleDriveAuthDto dto);

        Task<GoogleDriveAuthResult> DeleteGoogleDriveAuthById(long id);
    }
}
