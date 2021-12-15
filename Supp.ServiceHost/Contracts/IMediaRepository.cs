using Supp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
{
    public interface IMediaRepository
    {
        Task<MediaResult> GetAllMedia();

        Task<MediaResult> GetMediaById(long id);

        Task<MediaResult> UpdateMedia(MediaDto dto);

        Task<MediaResult> AddMedia(MediaDto dto);

        Task<MediaResult> AddRangeMedia(string dataJsonString);

        Task<MediaResult> DeleteMediaById(long id);

        Task<SongResult> ClearStructureMedia(string path);
    }
}
