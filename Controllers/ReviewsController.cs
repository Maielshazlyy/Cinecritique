
using CineCritique.DAL.DTOS;
using CineCritique.domain.Models;
using CineCritique.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineCritique.Api.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : Controller
    {
        private readonly IReview _reviewService;

        public ReviewsController(IReview reviewService)
        {
            _reviewService = reviewService;
        }

        // GET: api/review
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        // GET: api/review/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
                return NotFound();

            return Ok(review);
        }

        // GET: api/review/movie/3
        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetReviewsByMovieId(int movieId)
        {
            var reviews = await _reviewService.GetReviewsByMovieIdAsync(movieId);
            return Ok(reviews);
        }

        // POST: api/review
        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] ReviewDto reviewDto)
        {
            if (reviewDto == null)
            {
                return BadRequest("ReviewDto cannot be null.");
            }

            if (string.IsNullOrEmpty(reviewDto.Content))
            {
                return BadRequest("Review content cannot be empty.");
            }

            if (reviewDto.MovieId <= 0 || reviewDto.UserId <= 0)
            {
                return BadRequest("Invalid MovieId or UserId.");
            }

            // تأكد من أن التاريخ صالح
            if (reviewDto.DateCreated == default)
            {
                return BadRequest("Invalid date format.");
            }

            var review = new Reviews
            {
                MovieId = reviewDto.MovieId,
                UserId = reviewDto.UserId,
                Content = reviewDto.Content,
                DateCreated = reviewDto.DateCreated
            };


            await _reviewService.AddReviewAsync(review);
            return Ok(review);
        }

        // PUT: api/review/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewDto reviewDto)
        {
            var existingReview = await _reviewService.GetReviewByIdAsync(id);
            if (existingReview == null)
                return NotFound();

            existingReview.MovieId = reviewDto.MovieId;
            existingReview.UserId = reviewDto.UserId;
            existingReview.Content = reviewDto.Content;
            existingReview.DateCreated = reviewDto.DateCreated;

            await _reviewService.UpdateReviewAsync(existingReview);
            return Ok(existingReview);
        }

        // DELETE: api/review/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            await _reviewService.DeleteReviewAsync(id);
            return Ok(new { message = "Review deleted successfully." });
        }
    }
}
