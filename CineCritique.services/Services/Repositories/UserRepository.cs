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
    public class UserRepository : Repository<User>, IUserRepo
    {
        private readonly SqlServerDBContext _context;
        public UserRepository(SqlServerDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == username);
        }
        public async Task RemoveAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}

