using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CineCritique.services.Services
{
    public class ChatBotService : IChatBotService
    {
        private readonly IUnitOfWork _unitOfWork;


        public ChatBotService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ChatBot>> GetAllMessagesAsync()
        {
            return await _unitOfWork.ChatBot.GetAllAsync();
        }

        public async Task<ChatBot> GetMessageByIdAsync(int id)
        {
            return await _unitOfWork.ChatBot.GetByIdAsync(id);
        }

        public async Task AddMessageAsync(string userInput, string botResponse)
        {
            var message = new ChatBot
            {
                UserMessage = userInput,
                BotResponse = botResponse,
                Timestamp = DateTime.Now
            };

            await _unitOfWork.ChatBot.AddAsync(message);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var chatBot = await _unitOfWork.ChatBot.GetByIdAsync(id); // يجب استخدام _unitOfWork هنا
            if (chatBot != null)
            {
                await _unitOfWork.ChatBot.RemoveAsync(id); // استخدم RemoveAsync من ChatBotRepository
                await _unitOfWork.CommitAsync();
            }
        }
        public async Task<ChatBot> GetMessageByUserMessageAsync(string userMessage)
        {
            return await _unitOfWork.ChatBot.GetByUserMessageAsync(userMessage);
        }
    }
}       
