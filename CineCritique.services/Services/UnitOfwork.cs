using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.DAL;
using CineCritique.domain.Models;
using CineCritique.Repositories;
using CineCritique.services.Services.Repositories;
using CineCritique.services.Services.Repositories.Repository_InterFaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CineCritique.services.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlServerDBContext _context;
        

        public UnitOfWork(SqlServerDBContext context)
        {
            _context = context;
            
            Admins = new AdminRepository(_context);
            Users = new UserRepository(_context);
            Movies = new MovieRepository(_context);
            Reviews = new RevieweRepository(_context);
            Followers = new FollowerRepository(_context);
            CriticReviews = new CriticArticlesRepository(_context);
            ChatBot = new ChatBotRepository(_context);
            Playlists = new Playlistrepository(_context);

        }

        public IAdminRepo Admins { get; private set; }
        public IUserRepo Users { get; private set; }
        public ImovieRepo Movies { get; private set; }
        public IReviewrepo Reviews { get; private set; }
        public IFollowerRepo Followers { get; private set; }
        public ICriticArticleRepo CriticReviews { get; private set; }
        public IChatBotRepository ChatBot { get; private set; }
        public IPlaylistRepo Playlists { get; private set; }



        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task<int> CommitAsync()
{
    return await _context.SaveChangesAsync();
}

    }

}
