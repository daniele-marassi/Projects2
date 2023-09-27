using Supp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
{
    public interface ITokensRepository
    {
        Task<TokenResult> GetAllTokens();

        Task<TokenResult> GetTokensById(long id);

        Task<TokenResult> GetTokensByUserId(long userId);

        Task<TokenResult> UpdateToken(TokenDto dto);

        Task<TokenResult> AddToken(TokenDto dto);

        Task<TokenResult> DeleteTokenById(long id);

        Task<TokenResult> DeleteAllTokensByUserId(long userId);

        Task<TokenResult> CleanAndAddToken(TokenDto dto);
    }
}
