using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;

namespace CineCritique.services.Services
{
  public  interface IChatBotService
    {
        Task<IEnumerable<ChatBot>> GetAllMessagesAsync();
        Task<ChatBot> GetMessageByIdAsync(int id);
        Task AddMessageAsync(string userInput, string botResponse);
        
        Task<ChatBot> GetMessageByUserMessageAsync(string userMessage);
    }
}
