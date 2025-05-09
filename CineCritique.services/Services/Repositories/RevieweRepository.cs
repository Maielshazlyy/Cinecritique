using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.DAL;
using CineCritique.domain.Models;
using CineCritique.services.Services.Repositories.Repository_InterFaces;
using Microsoft.EntityFrameworkCore;

namespace CineCritique.services.Services.Repositories
{
   public class RevieweRepository : Repository<Reviews>, IReviewrepo
    {
        private readonly SqlServerDBContext _context;
        public RevieweRepository(SqlServerDBContext context) : base(context)
        {

            _context = context;
        }

        public async Task<IEnumerable<Reviews>> GetReviewsByMovieIdAsync(int movieId)
        {
            return await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .ToListAsync();
        }
        public void Remove(Reviews review)
        {
            _context.Reviews.Remove(review);
        }

    }
}
