using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CineCritique.DAL;
using CineCritique.domain.Models;
using CineCritique.services.Services.Repositories.Repository_InterFaces;
using Microsoft.EntityFrameworkCore;

namespace CineCritique.services.Services.Repositories
{
   public class Playlistrepository:IPlaylistRepo
    {
        private readonly SqlServerDBContext _context;

        public Playlistrepository(SqlServerDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Playlist>> GetAllAsync() =>
            await _context.Playlists.Include(p => p.Movies).ToListAsync();

        public async Task<Playlist> GetByIdAsync(int id) =>
            await _context.Playlists.Include(p => p.Movies).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<Playlist>> GetPlaylistsByUserIdAsync(int userId) =>
            await _context.Playlists.Where(p => p.UserId == userId).Include(p => p.Movies).ToListAsync();

        public async Task AddAsync(Playlist playlist) =>
            await _context.Playlists.AddAsync(playlist);

        public void Update(Playlist playlist) =>
            _context.Playlists.Update(playlist);

        public async Task RemoveAsync(Playlist playlist)
        {
            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task<Playlist> FindAsync(Expression<Func<Playlist, bool>> predicate) =>
            await _context.Playlists.FirstOrDefaultAsync(predicate);

        public async Task<Playlist> GetAsync(Expression<Func<Playlist, bool>> predicate) =>
            await _context.Playlists.FirstOrDefaultAsync(predicate);
    }
}
