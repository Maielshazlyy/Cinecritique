using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;

namespace CineCritique.services.Services
{
   public interface ICriticArticleService
    {
        Task<IEnumerable<CriticArticle>> GetAllCriticReviewsAsync();
        Task<CriticArticle> GetCriticReviewByIdAsync(int id);
        Task AddCriticReviewAsync(CriticArticle criticReview);
        Task UpdateCriticReviewAsync(CriticArticle criticReview);
        Task DeleteCriticReviewAsync(int id);
    }
}
