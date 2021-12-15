using Supp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
{
    public interface IUserRoleTypesRepository
    {
        Task<UserRoleTypeResult> GetAllUserRoleTypes();

        Task<UserRoleTypeResult> GetUserRoleTypesById(long id);

        Task<UserRoleTypeResult> UpdateUserRoleType(UserRoleTypeDto dto);

        Task<UserRoleTypeResult> AddUserRoleType(UserRoleTypeDto dto);

        Task<UserRoleTypeResult> DeleteUserRoleTypeById(long id);
    }
}
