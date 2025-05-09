using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;

namespace CineCritique.services.Services.Repositories.Repository_InterFaces
{
  public  interface ImovieRepo:iRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetAllAsync();
       // Task<Movie> GetByIdAsync(int id);
        //Task AddAsync(Movie movie);
       // void Update(Movie movie);
        //Task RemoveAsync(Movie movie);

    }
}
