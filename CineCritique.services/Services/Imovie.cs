using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;

namespace CineCritique.services.Services
{
   public interface Imovie
    {
            Task AddMovieAsync(Movie movie);
            Task DeleteMovieAsync(int movieId);
            Task UpdateMovieAsync(Movie movie);
            Task<IEnumerable<Movie>> GetAllMoviesAsync();
           Task<Movie> GetMovieByIdAsync(int id);
        Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre);
        
        Task<IEnumerable<Movie>> SearchMoviesByTitleAsync(string title);
            Task<IEnumerable<Movie>> GetTopRatedMoviesAsync();
            Task<IEnumerable<Movie>> GetMoviesFromExternalApiAsync();

    }

}

