using Mair.DigitalSuite.ServiceHost.Models;
using Mair.DigitalSuite.ServiceHost.Models.Dto.Auth;
using Mair.DigitalSuite.ServiceHost.Models.Result.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Contracts
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
