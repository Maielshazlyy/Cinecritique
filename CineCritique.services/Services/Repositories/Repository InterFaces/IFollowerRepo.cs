using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;

namespace CineCritique.services.Services.Repositories.Repository_InterFaces
{
  public  interface IFollowerRepo : iRepository<Follower>
    {
        Task<IEnumerable<Follower>> GetFollowersByUserIdAsync(int userId);
        Task<IEnumerable<Follower>> GetFollowingByUserIdAsync(int userId);
        

    }

}

