using CineCritique.domain.Models;
using CineCritique.DAL.DTOS;
using CineCritique.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineCritique.Api.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : Controller
    {
        private readonly Imovie _movieService;

        // Constructor
        public MovieController(Imovie movieService)
        {
            _movieService = movieService;
        }

        // إضافة فيلم
        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody] MovieDto movieDto)
        {
            try
            {
                if (movieDto == null)
                {
                    return BadRequest("Movie cannot be null.");
                }

                // تحويل MovieDto إلى Movie Entity
                var movie = new Movie
                {
                    Title = movieDto.Title,
                    Genre = movieDto.Genre,
                    GenreId = movieDto.GenreId,
                    Year = movieDto.Year,
                    Description = movieDto.Description,
                    PosterUrl = movieDto.PosterUrl,
                    Rating = movieDto.Rating
                };

                await _movieService.AddMovieAsync(movie);
                return CreatedAtAction(nameof(GetMoviesFromExternalApi), new { id = movie.Id }, movie);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // حذف فيلم
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            await _movieService.DeleteMovieAsync(id);
            return NoContent();
        }

        // تحديث فيلم
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieDto movieDto)
        {
            if (id != movieDto.Id)
            {
                return BadRequest("Movie ID mismatch.");
            }

            var existingMovie = await _movieService.GetMovieByIdAsync(id);
            if (existingMovie == null)
            {
                return NotFound();
            }

            // تحويل MovieDto إلى Movie Entity
            var movie = new Movie
            {
                Id = id,
                Title = movieDto.Title,
                Genre = movieDto.Genre,
                GenreId = movieDto.GenreId,
                Year = movieDto.Year,
                Description = movieDto.Description,
                PosterUrl = movieDto.PosterUrl,
                Rating = movieDto.Rating
            };

            await _movieService.UpdateMovieAsync(movie);
            return NoContent();
        }

        // جلب جميع الأفلام
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return Ok(movies);
        }

        // جلب فيلم بواسطة ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // جلب أفلام حسب التصنيف
        [HttpGet("genre/{genre}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByGenre(string genre)
        {
            var movies = await _movieService.GetMoviesByGenreAsync(genre);
            return Ok(movies);
        }

        // بحث عن أفلام حسب العنوان
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Movie>>> SearchMoviesByTitle([FromQuery] string title)
        {
            var movies = await _movieService.SearchMoviesByTitleAsync(title);
            return Ok(movies);
        }

        // جلب الأفلام الأعلى تقييماً
        [HttpGet("top-rated")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetTopRatedMovies()
        {
            var movies = await _movieService.GetTopRatedMoviesAsync();
            return Ok(movies);
        }

        // جلب أفلام من API خارجي
        [HttpGet("external")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesFromExternalApi()
        {
            var movies = await _movieService.GetMoviesFromExternalApiAsync();
            return Ok(movies);
        }
    }
}
