using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CineCritique.DAL.DTOS;
using CineCritique.domain.Models;
using CineCritique.services.Services.Repositories.Repository_InterFaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CineCritique.services.Services
{
   public class Authservice:IAuthService   
   {
        private readonly IUserRepo _userRepo;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        public Authservice(IUserRepo userRepo, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepo.GetUserByUsernameAsync(loginDto.Username);
            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed) return null;

            // إنشاء التوكن (Token) زي ما شرحنا في الأول
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userRepo.GetUserByUsernameAsync(registerDto.Username);
            if (existingUser != null) return false; // إذا كان المستخدم موجود بالفعل

            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                Role = registerDto.Role
               , // تصحيح هذا السطر
                IsActive = true
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);

            await _userRepo.AddAsync(user); // إضافة المستخدم لقاعدة البيانات
            return true;
        }
    }
}
