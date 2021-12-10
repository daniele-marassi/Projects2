using Supp.ServiceHost.Contracts;
using SuppModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Services.SignalR
{
    public class Hub: Hub<IHub>
    {
        public async Task SendHubDataToClients(List<HubDataDto> hubDataDto)
        {
            await Clients.All.ShowHubData(hubDataDto);
        }
    }
}
