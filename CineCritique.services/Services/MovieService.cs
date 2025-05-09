using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using CineCritique.domain.Models;
using System.Linq;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CineCritique.services.Services
{
    public class MovieService : Imovie
    {
        private Dictionary<int, Movie> _moviesCache = new Dictionary<int, Movie>();
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        // Constructor
        public MovieService(IUnitOfWork unitOfWork, HttpClient httpClient, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        // إضافة فيلم
        public async Task AddMovieAsync(Movie movie)

        {
            if (string.IsNullOrEmpty(movie.PosterUrl))
            {
                throw new ArgumentException("PosterUrl cannot be null or empty.");
            }
            movie.Id = 0; // Reset ID to let the database assign it
            await _unitOfWork.Movies.AddAsync(movie);
            await _unitOfWork.CommitAsync();
        }

        // حذف فيلم
        public async Task DeleteMovieAsync(int movieId)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(movieId);
            if (movie != null)
            {
                // إضافة حذف الفيلم من القاموس (إن كان موجودًا)
                if (_moviesCache.ContainsKey(movieId))
                {
                    _moviesCache.Remove(movieId);
                }

                // حذف الفيلم من قاعدة البيانات
                _unitOfWork.Movies.RemoveAsync(movie);
                await _unitOfWork.CommitAsync();
            }
        }

        // تحديث فيلم
        public async Task UpdateMovieAsync(Movie movie)
        {
            var existingMovie = await _unitOfWork.Movies.GetByIdAsync(movie.Id);
            if (existingMovie != null)
            {
                existingMovie.Title = movie.Title;
                existingMovie.Description = movie.Description;
                existingMovie.GenreId = movie.GenreId;

                _unitOfWork.Movies.Update(existingMovie);
                await _unitOfWork.CommitAsync();
            }
        }
        public async Task<Movie> GetMovieByIdAsync(int movieId)
        {
            // التأكد من أن الفيلم في الـ cache أولاً
            if (_moviesCache.ContainsKey(movieId))
            {
                return _moviesCache[movieId];
            }

            // إذا لم يكن الفيلم في الـ cache، جلبه من قاعدة البيانات
            var movie = await _unitOfWork.Movies.GetByIdAsync(movieId);

            // إذا وجد الفيلم، أضفه للـ cache
            if (movie != null && !_moviesCache.ContainsKey(movieId))
            {
                _moviesCache[movieId] = movie;
            }

            return movie;
        }
        // جلب جميع الأفلام
        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _unitOfWork.Movies.GetAllAsync();
        }

        // جلب فيلم بواسطة ID
       
        public async Task<int> GetGenreIdByNameAsync(string genreName)
        {
            string apiKey = _configuration["ExternalApi:ApiKey"];
            string baseUrl = _configuration["ExternalApi:BaseUrl"];

            // الطلب لجلب قائمة التصنيفات
            string requestUrl = $"{baseUrl}/genre/movie/list?api_key={apiKey}&language=en-US";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            string responseData = await response.Content.ReadAsStringAsync();

            var genreApiResponse = JsonConvert.DeserializeObject<GenreApiResponse>(responseData);

            // البحث عن الـ genreId بناءً على اسم التصنيف
            var genre = genreApiResponse.Genres.FirstOrDefault(g => g.Name.Equals(genreName, StringComparison.OrdinalIgnoreCase));

            return genre?.Id ?? -1; // إرجاع الـ genreId أو -1 إذا لم يتم العثور عليه
        }

        // جلب أفلام بناءً على التصنيف
        public async Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre)
        {
            string apiKey = _configuration["ExternalApi:ApiKey"];
            string baseUrl = _configuration["ExternalApi:BaseUrl"];
            string imageBaseUrl = _configuration["ExternalApi:ImageBaseUrl"];

            // الحصول على الـ genreId باستخدام اسم التصنيف
            int genreId = await GetGenreIdByNameAsync(genre);

            if (genreId == -1)
            {
                throw new ArgumentException("Invalid genre name.");
            }

            // بناء الـ URL باستخدام genreId
            string requestUrl = $"{baseUrl}/discover/movie?api_key={apiKey}&with_genres={genreId}&language=en-US&page=1";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            string responseData = await response.Content.ReadAsStringAsync();

            var movieApiResponse = JsonConvert.DeserializeObject<MovieApiResponse>(responseData);
            var movieList = new List<Movie>();

            foreach (var apiMovie in movieApiResponse.Results)
            {
                string genreName = "Unknown";
                // تفصيل الفيلم
                string detailUrl = $"{baseUrl}/movie/{apiMovie.Id}?api_key={apiKey}&language=en-US";
                HttpResponseMessage detailResponse = await _httpClient.GetAsync(detailUrl);

                if (detailResponse.IsSuccessStatusCode)
                {
                    var detailData = await detailResponse.Content.ReadAsStringAsync();
                    var detail = JsonConvert.DeserializeObject<MovieDetailsResponse>(detailData);
                    if (detail.Genres != null && detail.Genres.Any())
                    {
                        genreName = detail.Genres.First().Name;
                    }
                }

                var movie = new Movie
                {
                    Id = apiMovie.Id,
                    Title = apiMovie.Title ?? "Unknown",
                    Description = apiMovie.Overview ?? "No description available",
                    Genre = genreName,
                    Rating = apiMovie.VoteAverage,
                    Year = !string.IsNullOrEmpty(apiMovie.ReleaseDate) ? DateTime.Parse(apiMovie.ReleaseDate).Year : 0,
                    PosterUrl = !string.IsNullOrEmpty(apiMovie.PosterPath)
                        ? $"{imageBaseUrl}{apiMovie.PosterPath}"
                        : "https://example.com/default_image.jpg"
                };

                movieList.Add(movie);
            }

            return movieList;
        }



        // بحث عن أفلام بواسطة العنوان
        public async Task<IEnumerable<Movie>> SearchMoviesByTitleAsync(string title)
        {
            string apiKey = _configuration["ExternalApi:ApiKey"];
            string baseUrl = _configuration["ExternalApi:BaseUrl"];
            string imageBaseUrl = _configuration["ExternalApi:ImageBaseUrl"];
            string encodedTitle = Uri.EscapeDataString(title);
            string requestUrl = $"{baseUrl}/search/movie?api_key={apiKey}&language=en-US&query={Uri.EscapeDataString(encodedTitle)}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            string responseData = await response.Content.ReadAsStringAsync();

            var movieApiResponse = JsonConvert.DeserializeObject<MovieApiResponse>(responseData);
            var movieList = new List<Movie>();

            foreach (var apiMovie in movieApiResponse.Results)
            {
                // 🔄 Call for genre details
                string detailUrl = $"{baseUrl}/{apiMovie.Id}?api_key={apiKey}&language=en-US";
                HttpResponseMessage detailResponse = await _httpClient.GetAsync(detailUrl);

                string genreName = "Unknown";

                if (detailResponse.IsSuccessStatusCode)
                {
                    var detailData = await detailResponse.Content.ReadAsStringAsync();
                    var detail = JsonConvert.DeserializeObject<MovieDetailsResponse>(detailData);
                    if (detail.Genres != null && detail.Genres.Any())
                    {
                        genreName = detail.Genres.First().Name;
                    }
                }

                var movie = new Movie
                {
                    Id = apiMovie.Id,
                    Title = apiMovie.Title ?? "Unknown",
                    Description = apiMovie.Overview ?? "No description available",
                    Genre = genreName,
                    Rating = apiMovie.VoteAverage,
                    Year = !string.IsNullOrEmpty(apiMovie.ReleaseDate) ? DateTime.Parse(apiMovie.ReleaseDate).Year : 0,
                    PosterUrl = !string.IsNullOrEmpty(apiMovie.PosterPath)
                        ? $"{imageBaseUrl}{apiMovie.PosterPath}"
                        : "https://example.com/default_image.jpg"
                };

                if (!_moviesCache.ContainsKey(movie.Id))
                {
                    _moviesCache[movie.Id] = movie;
                    await AddMovieAsync(movie);
                }

                movieList.Add(movie);
            }

            return movieList;
        }

        // جلب الأفلام الأعلى تقييمًا
        public async Task<IEnumerable<Movie>> GetTopRatedMoviesAsync()
        {
            string apiKey = _configuration["ExternalApi:ApiKey"];
            string baseUrl = _configuration["ExternalApi:BaseUrl"];
            string imageBaseUrl = _configuration["ExternalApi:ImageBaseUrl"];
            string requestUrl = $"{baseUrl}/movie/top_rated?api_key={apiKey}&language=en-US&page=1";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            string responseData = await response.Content.ReadAsStringAsync();

            var movieApiResponse = JsonConvert.DeserializeObject<MovieApiResponse>(responseData);
            var movieList = new List<Movie>();

            foreach (var apiMovie in movieApiResponse.Results)
            {
                string detailUrl = $"{baseUrl}/{apiMovie.Id}?api_key={apiKey}&language=en-US";
                HttpResponseMessage detailResponse = await _httpClient.GetAsync(detailUrl);

                string genreName = "Unknown";

                if (detailResponse.IsSuccessStatusCode)
                {
                    var detailData = await detailResponse.Content.ReadAsStringAsync();
                    var detail = JsonConvert.DeserializeObject<MovieDetailsResponse>(detailData);
                    if (detail.Genres != null && detail.Genres.Any())
                    {
                        genreName = detail.Genres.First().Name;
                    }
                }

                var movie = new Movie
                {
                    Id = apiMovie.Id,
                    Title = apiMovie.Title ?? "Unknown",
                    Description = apiMovie.Overview ?? "No description available",
                    Genre = genreName,
                    Rating = apiMovie.VoteAverage,
                    Year = !string.IsNullOrEmpty(apiMovie.ReleaseDate) ? DateTime.Parse(apiMovie.ReleaseDate).Year : 0,
                    PosterUrl = !string.IsNullOrEmpty(apiMovie.PosterPath)
                        ? $"{imageBaseUrl}{apiMovie.PosterPath}"
                        : "https://example.com/default_image.jpg"
                };

                if (!_moviesCache.ContainsKey(movie.Id))
                {
                    _moviesCache[movie.Id] = movie;
                    await AddMovieAsync(movie); // هنا بتضبطي الـ ID تلقائي
                }

                movieList.Add(movie);
            }

            return movieList;
        }

        // جلب أفلام من API خارجي
        public async Task<IEnumerable<Movie>> GetMoviesFromExternalApiAsync()
        {
            string apiKey = _configuration["ExternalApi:ApiKey"];
            string baseUrl = _configuration["ExternalApi:BaseUrl"];
            string imageBaseUrl = _configuration["ExternalApi:ImageBaseUrl"];
            string requestUrl = $"{baseUrl}/movie/popular?api_key={apiKey}&language=en-US&page=1";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            string responseData = await response.Content.ReadAsStringAsync();

            var movieApiResponse = JsonConvert.DeserializeObject<MovieApiResponse>(responseData);
            var movieList = new List<Movie>();

            foreach (var apiMovie in movieApiResponse.Results)
            {
                // 🔄 Call for movie details
                string detailUrl = $"{baseUrl}/{apiMovie.Id}?api_key={apiKey}&language=en-US";
                HttpResponseMessage detailResponse = await _httpClient.GetAsync(detailUrl);

                string genreName = "Unknown";

                if (detailResponse.IsSuccessStatusCode)
                {
                    var detailData = await detailResponse.Content.ReadAsStringAsync();
                    var detail = JsonConvert.DeserializeObject<MovieDetailsResponse>(detailData);
                    if (detail.Genres != null && detail.Genres.Any())
                    {
                        genreName = detail.Genres.First().Name;
                    }
                }

                var movie = new Movie
                {
                    Id = apiMovie.Id,
                    Title = apiMovie.Title ?? "Unknown",
                    Description = apiMovie.Overview ?? "No description available",
                    Genre = genreName,
                    Rating = apiMovie.VoteAverage,
                    Year = !string.IsNullOrEmpty(apiMovie.ReleaseDate) ? DateTime.Parse(apiMovie.ReleaseDate).Year : 0,
                    PosterUrl = !string.IsNullOrEmpty(apiMovie.PosterPath)
                        ? $"{imageBaseUrl}{apiMovie.PosterPath}"
                        : "https://example.com/default_image.jpg"
                };

                if (!_moviesCache.ContainsKey(movie.Id))
                {
                    _moviesCache[movie.Id] = movie;
                    await AddMovieAsync(movie);
                }

                movieList.Add(movie);
            }

            return movieList;
        }



    }

}

// نموذج Response من الـ API
public class MovieApiResponse
{
    public List<MovieApiResult> Results { get; set; }
}

public class MovieApiResult
{
   
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }
    }
public class MovieDetailsResponse
{
    [JsonProperty("genres")]
    public List<Genre> Genres { get; set; }
}

public class Genre
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}
public class GenreApiResponse
{
    [JsonProperty("genres")]
    public List<Genre> Genres { get; set; }
}

