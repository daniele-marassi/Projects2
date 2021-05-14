using Mair.DigitalSuite.TagDispatcher.Models;
using Mair.DigitalSuite.TagDispatcher.Models.Dto.Automation;
using Mair.DigitalSuite.TagDispatcher.Models.Result.Automation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.TagDispatcher.Contracts
{
    public interface IPlcDataRepository: IDisposable
    {
        Task<PlcDataResult> GetAllPlcData();

        Task<PlcDataResult> UpdatePlcData(PlcDataDto plcDataDto);
    }
}
