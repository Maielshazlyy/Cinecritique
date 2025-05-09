using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;
using CineCritique.services.Services.Repositories;
using CineCritique.services.Services.Repositories.Repository_InterFaces;

namespace CineCritique.services.Services
{
    public interface IUnitOfWork
    {
        IFollowerRepo Followers { get; }
        IReviewrepo Reviews { get; }
        ICriticArticleRepo CriticReviews { get; }
        ImovieRepo Movies { get; }
        IUserRepo Users { get; }
        IAdminRepo Admins { get; }
        IChatBotRepository ChatBot { get; }
        IPlaylistRepo Playlists { get; }



        Task<int> CommitAsync();

        Task<int> CompleteAsync();
    }
}
       
    

