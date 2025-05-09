using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;

namespace CineCritique.services.Services
{
  public  class ReviewService:IReview
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Reviews>> GetAllReviewsAsync()
        {
            return await _unitOfWork.Reviews.GetAllAsync();
        }

        public async Task<Reviews> GetReviewByIdAsync(int id)
        {
            return await _unitOfWork.Reviews.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Reviews>> GetReviewsByMovieIdAsync(int movieId)
        {
            return await _unitOfWork.Reviews.GetReviewsByMovieIdAsync(movieId);
        }

        public async Task AddReviewAsync(Reviews review)
        {
            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateReviewAsync(Reviews review)
        {
            _unitOfWork.Reviews.Update(review);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review != null)
            {
                await _unitOfWork.Reviews.RemoveAsync(review);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
