using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;

namespace CineCritique.services.Services
{
   public class CriticArticalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CriticArticalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CriticArticle>> GetAllCriticReviewsAsync()
        {
            return await _unitOfWork.CriticReviews.GetAllAsync();
        }

        public async Task<CriticArticle> GetCriticReviewByIdAsync(int id)
        {
            return await _unitOfWork.CriticReviews.GetByIdAsync(id);
        }

        public async Task AddCriticReviewAsync(CriticArticle criticReview)
        {
            await _unitOfWork.CriticReviews.AddAsync(criticReview);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateCriticReviewAsync(CriticArticle criticReview)
        {
            _unitOfWork.CriticReviews.Update(criticReview);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteCriticReviewAsync(int id)
        {
            var criticReview = await _unitOfWork.CriticReviews.GetByIdAsync(id);
            if (criticReview != null)
            {
                 _unitOfWork.CriticReviews.Remove(criticReview);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
