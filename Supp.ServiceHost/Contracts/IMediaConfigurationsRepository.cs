using Supp.ServiceHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
{
    public interface IMediaConfigurationsRepository
    {
        Task<MediaConfigurationResult> GetAllMediaConfigurations();

        Task<MediaConfigurationResult> GetMediaConfigurationsById(long id);

        Task<MediaConfigurationResult> UpdateMediaConfiguration(MediaConfigurationDto dto);

        Task<MediaConfigurationResult> AddMediaConfiguration(MediaConfigurationDto dto);

        Task<MediaConfigurationResult> DeleteMediaConfigurationById(long id);
    }
}
