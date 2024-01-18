using Supp.Models;
using System.Threading.Tasks;

namespace Supp.Interfaces
{
    public interface IGoogleAccountsRepository
    {
        /// <summary>
        /// Get All GoogleAccounts
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<GoogleAccountResult> GetAllGoogleAccounts(string token);

        /// <summary>
        /// Get GoogleAccounts By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<GoogleAccountResult> GetGoogleAccountsById(long id, string token);

        /// <summary>
        /// Update GoogleAccount
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<GoogleAccountResult> UpdateGoogleAccount(GoogleAccountDto dto, string token);

        /// <summary>
        /// Add GoogleAccount
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<GoogleAccountResult> AddGoogleAccount(GoogleAccountDto dto, string token);

        /// <summary>
        /// Delete GoogleAccount By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<GoogleAccountResult> DeleteGoogleAccountById(long id, string token);
    }
}
