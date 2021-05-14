using Mair.DigitalSuite.Dashboard.Models;
using Mair.DigitalSuite.Dashboard.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.Dashboard.Contracts
{
    interface IPlcHubClient
    {
        Task Execute(List<DashBoardDataDto> DashBoardDataDto);
    }
}
