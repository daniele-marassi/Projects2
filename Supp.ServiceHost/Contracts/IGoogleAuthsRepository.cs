using Supp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
{
    public interface IGoogleAuthsRepository
    {
        Task<GoogleAuthResult> GetAllGoogleAuths();

        Task<GoogleAuthResult> GetGoogleAuthsById(long id);

        Task<GoogleAuthResult> UpdateGoogleAuth(GoogleAuthDto dto);

        Task<GoogleAuthResult> AddGoogleAuth(GoogleAuthDto dto);

        Task<GoogleAuthResult> DeleteGoogleAuthById(long id);
    }
}
