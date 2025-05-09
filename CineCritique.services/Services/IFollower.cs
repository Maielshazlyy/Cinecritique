using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;

namespace CineCritique.services.Services
{
    public interface IFollower
    {

        Task FollowAsync(string followerId, string followingId);
        Task UnfollowAsync(int followerId, int followingId);
        Task<IEnumerable<User>> GetFollowersAsync(int userId);
        Task<IEnumerable<User>> GetFollowingAsync(int userId);



    }
}


