using Mair.DigitalSuite.ServiceHost.Contracts;
using Mair.DigitalSuite.ServiceHost.Models;
using Mair.DigitalSuite.ServiceHost.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Services.SignalR
{
    public class PlcHub: Hub<IPlcHub>
    {
        public async Task SendDashboardDataToClients(List<DashboardDataDto> dashboardDataDto)
        {
            await Clients.All.ShowDashboardData(dashboardDataDto);
        }
    }
}
