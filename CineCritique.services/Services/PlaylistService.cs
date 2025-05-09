using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.DAL.DTOS;
using CineCritique.domain.Models;

namespace CineCritique.services.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlaylistService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Playlist>> GetAllPlaylistsAsync() =>
            await _unitOfWork.Playlists.GetAllAsync();

        public async Task<Playlist> GetPlaylistByIdAsync(int id) =>
            await _unitOfWork.Playlists.GetByIdAsync(id);

        public async Task<IEnumerable<Playlist>> GetPlaylistsByUserIdAsync(int userId) =>
            await _unitOfWork.Playlists.GetPlaylistsByUserIdAsync(userId);

        public async Task AddPlaylistAsync(Playlist playlist)
        {
            await _unitOfWork.Playlists.AddAsync(playlist);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdatePlaylistAsync(Playlist playlist)
        {
            _unitOfWork.Playlists.Update(playlist);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> DeletePlaylistAsync(int id)
        {
            var playlist = await _unitOfWork.Playlists.GetByIdAsync(id);
            if (playlist == null) return false;

            await _unitOfWork.Playlists.RemoveAsync(playlist);
            await _unitOfWork.CommitAsync();
            return true;  // إرجاع true في حالة الحذف الناجح
        }

        public async Task<Playlist> CreatePlaylistAsync(CreatePlaylistDto dto)
        {
            // Ensure the DTO is not null and contains the necessary properties.
            if (dto == null)
            {
                // You can throw an exception or return null (but null should be avoided for async methods).
                throw new ArgumentNullException(nameof(dto), "The playlist data is null.");
            }

            var playlist = new Playlist
            {
                // Map the properties from the DTO to the Playlist entity.
                Name = dto.Name,
               
                UserId = dto.UserId,
                // You can add other properties as necessary.
            };

            // Add the playlist to the unit of work and commit the changes.
            await _unitOfWork.Playlists.AddAsync(playlist);
            await _unitOfWork.CommitAsync();

            return playlist;  // Ensure the playlist is returned.
        }
        public async Task<bool> AddMovieToPlaylistAsync(int playlistId, int movieId)
        {
            var playlist = await _unitOfWork.Playlists.GetByIdAsync(playlistId);
            if (playlist == null) return false;

            var movie = await _unitOfWork.Movies.GetByIdAsync(movieId);
            if (movie == null) return false;

            // يمكن أن تقوم بتحديث علاقة الـ Playlist والفيلم هنا باستخدام مثلاً جدول مشترك أو طرق أخرى
            playlist.Movies.Add(movie);  // فرضًا أن هناك علاقة بين Playlist و Movies

            await _unitOfWork.CommitAsync();
            return true;
        }

        // إزالة فيلم من الـ Playlist
        public async Task<bool> RemoveMovieFromPlaylistAsync(int playlistId, int movieId)
        {
            var playlist = await _unitOfWork.Playlists.GetByIdAsync(playlistId);
            if (playlist == null) return false;

            var movie = playlist.Movies.FirstOrDefault(m => m.Id == movieId);
            if (movie == null) return false;

            playlist.Movies.Remove(movie);  // إزالة الفيلم من الـ Playlist

            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}



