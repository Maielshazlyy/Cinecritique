using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.DAL;
using CineCritique.domain.Models;
using CineCritique.services.Services.Repositories.Repository_InterFaces;
using Microsoft.EntityFrameworkCore;

namespace CineCritique.services.Services.Repositories
{
   public class FollowerRepository:Repository<Follower>, IFollowerRepo
{
        private readonly SqlServerDBContext _context;
    public FollowerRepository(SqlServerDBContext context) : base(context)
    {
            _context = context;
    }

        public async Task<IEnumerable<Follower>> GetFollowersByUserIdAsync(int userId)
        {
            return await _context.Followers
                .Include(f => f.FollowerUser)
                .Where(f => Convert.ToInt32(f.FollowingUserId) == userId)
                .ToListAsync();
        }


        public async Task<IEnumerable<Follower>> GetFollowingByUserIdAsync(int userId)
    {
        return await _context.Followers
            .Where(f => Convert.ToInt32(f.FollowerUserId) == userId)
            .ToListAsync();
    }
    
    }
}
