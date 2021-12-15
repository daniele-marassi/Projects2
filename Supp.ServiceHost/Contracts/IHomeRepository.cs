using Supp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
{
    public interface IHomeRepository
    {
        Task<ApiResult> GetAllApi(HttpRequest request, string nameSpaceControllers);
    }
}
