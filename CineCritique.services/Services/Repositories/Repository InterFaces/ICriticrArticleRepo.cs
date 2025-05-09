using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;

namespace CineCritique.services.Services.Repositories.Repository_InterFaces
{
    public interface ICriticArticleRepo : iRepository<CriticArticle>
    {
        Task<IEnumerable<CriticArticle>> GetCriticalReviewsByUserIdAsync(int userId);
        void Remove(CriticArticle article);
    }
}
