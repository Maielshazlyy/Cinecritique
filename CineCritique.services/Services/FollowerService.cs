using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.DAL;
using CineCritique.domain.Models;

using Microsoft.EntityFrameworkCore;


namespace CineCritique.services.Services
{
   public class FollowerService:IFollower
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlServerDBContext _context;

        public FollowerService(IUnitOfWork unitOfWork, SqlServerDBContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task FollowAsync(string followerId, string followingId)
        {
            var follow = new Follower
            {
                FollowerUserId = followerId,
                FollowingUserId = followingId
            };
            await _unitOfWork.Followers.AddAsync(follow);
            await _unitOfWork.CommitAsync();
        }

        public async Task UnfollowAsync(int followerId, int followingId)
        {
            var follow = await _unitOfWork.Followers.GetAsync(f => Convert.ToInt32(f.FollowerUserId) == followerId && Convert.ToInt32(f.FollowerUserId) == followingId);
            if (follow != null)
            {
                await _unitOfWork.Followers.RemoveAsync(follow);
                await _unitOfWork.CommitAsync();
            }
        }
        public async Task<IEnumerable<User>> GetFollowersAsync(int userId)
        {
            var followers = await _context.Followers
                .Include(f => f.FollowerUser)
                .Where(f => Convert.ToInt32(f.FollowingUserId) == userId)
                .Select(f => f.FollowerUser)
                .ToListAsync();

            return followers;
        }


        public async Task<IEnumerable<User>> GetFollowingAsync(int userId)
        {
            var following = await _context.Followers
                .Include(f => f.FollowingUser)
                .Where(f => Convert.ToInt32(f.FollowerUserId) == userId)
                .Select(f => f.FollowingUser)
                .ToListAsync();

            return following;
        }




        
        }

    }


