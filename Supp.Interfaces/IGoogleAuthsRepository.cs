using Supp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Supp.Interfaces
{
    public interface IGoogleAuthsRepository
    {
        /// <summary>
        /// Get All GoogleAuths
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<GoogleAuthResult> GetAllGoogleAuths(string token);

        /// <summary>
        /// Get GoogleAuths By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<GoogleAuthResult> GetGoogleAuthsById(long id, string token);

        /// <summary>
        /// Update GoogleAuth
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<GoogleAuthResult> UpdateGoogleAuth(GoogleAuthDto dto, string token);

        /// <summary>
        /// Add GoogleAuth
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<GoogleAuthResult> AddGoogleAuth(GoogleAuthDto dto, string token);

        /// <summary>
        /// Delete GoogleAuth By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<GoogleAuthResult> DeleteGoogleAuthById(long id, string token);
    }
}
