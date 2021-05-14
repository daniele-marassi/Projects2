using Mair.DigitalSuite.ServiceHost.Models;
using Mair.DigitalSuite.ServiceHost.Models.Dto.Auth;
using Mair.DigitalSuite.ServiceHost.Models.Result.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Contracts
{
    public interface IUserRolesRepository
    {
        Task<UserRoleResult> GetAllUserRoles();

        Task<UserRoleResult> GetUserRolesById(long id);

        Task<UserRoleResult> UpdateUserRole(UserRoleDto dto);

        Task<UserRoleResult> AddUserRole(UserRoleDto dto);

        Task<UserRoleResult> DeleteUserRoleById(long id);
    }
}
