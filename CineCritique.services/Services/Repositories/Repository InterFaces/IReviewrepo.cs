using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;

namespace CineCritique.services.Services.Repositories.Repository_InterFaces
{
    public interface IReviewrepo : iRepository<Reviews>
    {
        Task<IEnumerable<Reviews>> GetReviewsByMovieIdAsync(int movieId);
        void Remove(Reviews review);
    }
}

