using Mair.DigitalSuite.ServiceHost.Models;
using Mair.DigitalSuite.ServiceHost.Models.Dto.Automation;
using Mair.DigitalSuite.ServiceHost.Models.Result.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Contracts
{
    public interface INodesRepository
    {
        Task<NodeResult> GetAllNodes();

        Task<NodeResult> GetNodesById(long id);

        Task<NodeResult> UpdateNode(NodeDto dto);

        Task<NodeResult> AddNode(NodeDto dto);

        Task<NodeResult> DeleteNodeById(long id);
    }
}
