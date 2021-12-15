using Supp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
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
