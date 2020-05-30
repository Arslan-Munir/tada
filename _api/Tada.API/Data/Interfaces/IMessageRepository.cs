using System.Collections.Generic;
using System.Threading.Tasks;
using Tada.API.Helpers;
using Tada.API.Models;

namespace Tada.API.Data.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageThread(int senderId, int receiverId);
    }
}