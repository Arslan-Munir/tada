using System.Threading.Tasks;
using Tada.API.Models;

namespace Tada.API.Data.Interfaces
{
    public interface ILikeRepository: IRepository<Like>
    {
        Task<Like> DoesLike(int likerId, int likeeId);
    }
}