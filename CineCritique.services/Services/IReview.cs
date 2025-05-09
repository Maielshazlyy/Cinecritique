using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;

namespace CineCritique.services.Services
{
    public interface IReview
    {
       
        
            Task<IEnumerable<Reviews>> GetAllReviewsAsync();
            Task<Reviews> GetReviewByIdAsync(int id);
            Task<IEnumerable<Reviews>> GetReviewsByMovieIdAsync(int movieId);
            Task AddReviewAsync(Reviews review);
            Task UpdateReviewAsync(Reviews review);
            Task DeleteReviewAsync(int id);
        }

    }

