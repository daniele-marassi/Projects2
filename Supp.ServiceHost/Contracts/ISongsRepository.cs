using Supp.ServiceHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supp.ServiceHost.Contracts
{
    public interface ISongsRepository
    {
        Task<SongResult> GetAllSongs();

        Task<SongResult> GetSongsById(long id);

        Task<SongResult> UpdateSong(SongDto dto);

        Task<SongResult> AddSong(SongDto dto);

        Task<SongResult> DeleteSongById(long id);

        Task<SongResult> ClearSongs();
    }
}
