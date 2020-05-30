using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tada.API.Data.Interfaces;
using Tada.API.Helpers;
using Tada.API.Models;

namespace Tada.API.Data
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        private readonly DataContext _context;

        public MessageRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _context.Messages
                .Include(m => m.Sender).ThenInclude(u => u.Photos)
                .Include(m => m.Receiver).ThenInclude(u => u.Photos)
                .AsQueryable();
            
            switch(messageParams.MessagesType)
            {
                case "received":
                    messages = messages.Where(m => m.ReceiverId == messageParams.UserId && !m.ReceiverDeleted);
                    break;
                
                case "sent":
                    messages = messages.Where(m => m.SenderId == messageParams.UserId && !m.SenderDeleted);
                    break;
                
                default:
                    messages = messages.Where(m => m.ReceiverId == messageParams.UserId && !m.ReceiverDeleted && m.IsRead == false);
                    break;
            }

            messages = messages.OrderByDescending(d => d.SentDate);
            return await PagedList<Message>.CreateAsync(messages, messageParams.CurrentPage, messageParams.ItemsPerPage);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int senderId, int receiverId)
        {
            return await _context.Messages
                .Include(m => m.Sender).ThenInclude(u => u.Photos)
                .Include(m => m.Receiver).ThenInclude(u => u.Photos)
                .Where(
                    m => m.ReceiverId == senderId && m.SenderId == receiverId && !m.ReceiverDeleted
                    || m.ReceiverId == receiverId && m.SenderId == senderId && !m.SenderDeleted)
                .ToListAsync();
        }
    }
}