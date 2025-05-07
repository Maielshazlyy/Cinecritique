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
   public class AdminRepository : Repository<Admin>, IAdminRepo
    {
        private readonly SqlServerDBContext _context;

        public AdminRepository(SqlServerDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Admin> GetAdminByUsernameAsync(string username)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.UserName == username);
        }

        public async Task<IEnumerable<Admin>> GetAllAdminsAsync()
        {
            return await _context.Admins.ToListAsync();
        }

        public async Task<bool> BanUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsBanned = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnbanUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsBanned = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveTopUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Role = "top user";
            await _context.SaveChangesAsync();
            return true;
        }
    
    }
}
