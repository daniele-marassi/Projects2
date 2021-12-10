using SuppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
{
    public interface IUsersRepository
    {
        Task<UserResult> GetAllUsers();

        Task<UserResult> GetUsersById(long id);

        Task<UserResult> UpdateUser(UserDto dto);

        Task<UserResult> AddUser(UserDto dto);

        Task<UserResult> DeleteUserById(long id);
    }
}
