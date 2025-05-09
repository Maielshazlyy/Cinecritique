using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;

namespace CineCritique.services.Services.Repositories.Repository_InterFaces
{
   public interface IAdminRepo:iRepository<Admin>
{
    Task<Admin> GetAdminByUsernameAsync(string username);
    Task<IEnumerable<Admin>> GetAllAdminsAsync();
    Task<bool> BanUserAsync(int userId);
    Task<bool> UnbanUserAsync(int userId);
    Task<bool> ApproveTopUserAsync(int userId);
}

   }

