using CineCritique.DAL.DTOS;
using CineCritique.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineCritique.Api.Controllers
{
    [ApiController]
    [Route("api/playlist")]
    public class Playlistcontroller:Controller
    {
        private readonly IPlaylistService _playlistService;

        public Playlistcontroller(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistDto dto)
        {
            if (dto == null)
                return BadRequest("Playlist data is null.");

            var playlist = await _playlistService.CreatePlaylistAsync(dto);
            return Ok(playlist); // إرجاع الـ Playlist المُنشأ
        }

        [HttpPost("add-movie")]
        public async Task<IActionResult> AddMovieToPlaylist([FromBody] AddMovieToPlaylistDto dto)
        {
            var result = await _playlistService.AddMovieToPlaylistAsync(dto.PlaylistId, dto.MovieId);
            return result ? Ok("Movie added.") : BadRequest("Failed to add movie.");
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserPlaylists(int userId)
        {
            var playlists = await _playlistService.GetPlaylistsByUserIdAsync(userId);
            return Ok(playlists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlaylistDetails(int id)
        {
            var playlist = await _playlistService.GetPlaylistByIdAsync(id);
            if (playlist == null) return NotFound();
            return Ok(playlist);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            var result = await _playlistService.DeletePlaylistAsync(id);
            return result ? Ok("Deleted.") : NotFound();
        }

        [HttpDelete("{playlistId}/movie/{movieId}")]
        public async Task<IActionResult> RemoveMovieFromPlaylist(int playlistId, int movieId)
        {
            var result = await _playlistService.RemoveMovieFromPlaylistAsync(playlistId, movieId);
            return result ? Ok("Movie removed.") : NotFound();
        }
    }
}
