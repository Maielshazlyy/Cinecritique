using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CineCritique.services.Services.Repositories.Repository_InterFaces
{
   public interface iRepository <T> where T : class
{
            Task<IEnumerable<T>> GetAllAsync();
            Task<T> GetByIdAsync(int id);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
            void Update(T entity);
        Task RemoveAsync(T entity);

    }
}

