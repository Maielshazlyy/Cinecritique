using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.DAL.DTOS;
using CineCritique.domain.Models;

namespace CineCritique.services.Services
{
   public interface IPlaylistService
    {
        Task<IEnumerable<Playlist>> GetAllPlaylistsAsync();
        Task<Playlist> GetPlaylistByIdAsync(int id);
        Task<IEnumerable<Playlist>> GetPlaylistsByUserIdAsync(int userId);
        Task AddPlaylistAsync(Playlist playlist);
        Task UpdatePlaylistAsync(Playlist playlist);
        Task <bool> DeletePlaylistAsync(int id);
        Task<Playlist> CreatePlaylistAsync(CreatePlaylistDto dto);
        Task<bool> AddMovieToPlaylistAsync(int playlistId, int movieId);
        Task<bool> RemoveMovieFromPlaylistAsync(int playlistId, int movieId);
    }
}
