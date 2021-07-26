using Tools.Songs.Models;
using System.Threading.Tasks;

namespace Tools.Songs.Contracts
{
    public interface ISongsRepository
    {
        Task<SongResult> GetAllSongs();

        Task<SongResult> GetSongsById(long id);

        Task<SongResult> UpdateSongs(SongDto dto);

        Task<SongResult> AddSongs(SongDto dto);

        Task<SongResult> DeleteSongsById(long id);

        Task<SongResult> ClearSongs();    
    }
}
