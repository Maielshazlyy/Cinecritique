using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;

namespace CineCritique.services.Services
{
   public class AdminService:IAdmin
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // إدارة المستخدمين
        public async Task DeactivateUserAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = false;
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task ActivateUserAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = true;
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user != null)
            {
                _unitOfWork.Users.Remove(user);
                await _unitOfWork.CommitAsync();
            }
        }

        // إدارة الأفلام
        public async Task AddMovieAsync(Movie movie)
        {
            await _unitOfWork.Movies.AddAsync(movie);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteMovieAsync(int movieId)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(movieId);
            if (movie != null)
            {
                _unitOfWork.Movies.RemoveAsync(movie);
                await _unitOfWork.CommitAsync();
            }
        }

        // إدارة المقالات النقدية
        public async Task DeleteCriticalArticleAsync(int articleId)
        {
            var article = await _unitOfWork.CriticReviews.GetByIdAsync(articleId);
            if (article != null)
            {
                _unitOfWork.CriticReviews.Remove(article);
                await _unitOfWork.CommitAsync();
            }
        }

        // إدارة المراجعات
        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId);
            if (review != null)
            {
                _unitOfWork.Reviews.Remove(review);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}

