using SuppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
{
    public interface IHub
    {
        Task ShowHubData(List<HubDataDto> hubDataDto);
    }
}
