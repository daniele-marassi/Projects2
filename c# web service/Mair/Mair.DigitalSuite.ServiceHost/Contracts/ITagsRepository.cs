using Mair.DigitalSuite.ServiceHost.Models;
using Mair.DigitalSuite.ServiceHost.Models.Dto.Automation;
using Mair.DigitalSuite.ServiceHost.Models.Result.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mair.DigitalSuite.ServiceHost.Contracts
{
    public interface ITagsRepository
    {
        Task<TagResult> GetAllTags();

        Task<TagResult> GetTagsById(long id);

        Task<TagResult> UpdateTag(TagDto dto);

        Task<TagResult> AddTag(TagDto dto);

        Task<TagResult> DeleteTagById(long id);
    }
}
