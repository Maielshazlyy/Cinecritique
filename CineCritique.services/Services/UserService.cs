using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.DAL.DTOS;
using CineCritique.domain.Models;
using Microsoft.AspNetCore.Identity;

namespace CineCritique.services.Services
{
    public class UserService : IUser
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _unitOfWork.Users.GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.Users.GetAllAsync();
        }

        public async Task AddUserAsync(User user)
        {
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                throw new ArgumentException("Password cannot be null or empty.");
            }
            user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user != null)
            {
                _unitOfWork.Users.Remove(user);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}