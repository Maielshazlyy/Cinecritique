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
    public class ChatBotRepository : Repository<ChatBot>, IChatBotRepository
    {
        private readonly SqlServerDBContext _context;
        public ChatBotRepository(SqlServerDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<ChatBot> GetByUserMessageAsync(string userMessage)
        {
            return await _context.ChatMessages
                .Where(c => c.UserMessage.Contains(userMessage)) // البحث داخل النص
                .FirstOrDefaultAsync();
        }
        public async Task RemoveAsync(int id)
        {
            var chatBot = await _context.ChatMessages.FindAsync(id);
            if (chatBot != null)
            {
                _context.ChatMessages.Remove(chatBot);
                await _context.SaveChangesAsync();
            }


        }

    }
}
