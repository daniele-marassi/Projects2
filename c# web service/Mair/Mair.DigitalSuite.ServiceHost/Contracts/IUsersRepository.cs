using Mair.DigitalSuite.ServiceHost.Models;
using Mair.DigitalSuite.ServiceHost.Models.Dto.Auth;
using Mair.DigitalSuite.ServiceHost.Models.Result.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Contracts
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
