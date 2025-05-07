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
   public  class CriticArticlesRepository:Repository<CriticArticle>, ICriticArticleRepo
{
        private readonly SqlServerDBContext _context;
    public CriticArticlesRepository(SqlServerDBContext context) : base(context)
    {
            _context = context;
    }

    public async Task<IEnumerable<CriticArticle>> GetCriticalReviewsByUserIdAsync(int userId)
    {
        return await _context.CriticalArticles
            .Where(cr => cr.UserId == userId)
            .ToListAsync();
    }
    public void Remove(CriticArticle criticArticle)
      {
            _context.CriticalArticles.Remove(criticArticle);
      }

    }
}
