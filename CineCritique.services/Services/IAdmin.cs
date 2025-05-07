using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;

namespace CineCritique.services.Services
{
   public interface IAdmin
    {
        Task DeactivateUserAsync(int userId);
        Task ActivateUserAsync(int userId);
        Task DeleteUserAsync(int userId);

        Task AddMovieAsync(Movie movie);
        Task DeleteMovieAsync(int movieId);

        Task DeleteCriticalArticleAsync(int articleId);
        Task DeleteReviewAsync(int reviewId);
    }
}
