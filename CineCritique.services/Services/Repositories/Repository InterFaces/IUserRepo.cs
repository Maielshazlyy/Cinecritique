using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;

namespace CineCritique.services.Services.Repositories.Repository_InterFaces
{
  public interface IUserRepo:iRepository<User>
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetUserByUsernameAsync(string username);
        void Remove(User user);
    }
}
