using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CineCritique.DAL; 
using CineCritique.domain.Models;
using CineCritique.services.Services.Repositories.Repository_InterFaces;
using System.Linq.Expressions;

namespace CineCritique.Repositories
{
    public class MovieRepository : ImovieRepo
    {
        private readonly SqlServerDBContext _context;

        public MovieRepository(SqlServerDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task AddAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
        }

        public void Update(Movie movie)
        {
            _context.Movies.Update(movie);
        }
        public async Task<Movie> FindAsync(Expression<Func<Movie, bool>> predicate)
        {
            return await _context.Movies.FirstOrDefaultAsync(predicate);
        }
        public async Task<Movie> GetAsync(Expression<Func<Movie, bool>> predicate)
        {
            return await _context.Movies.FirstOrDefaultAsync(predicate);
        }



        public async Task RemoveAsync(Movie entity)
        {
            _context.Movies.Remove(entity);
            await _context.SaveChangesAsync();
        }

    }
}

