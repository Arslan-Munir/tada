using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tada.API.Data.Interfaces;
using Tada.API.Models;

namespace Tada.API.Data
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        private readonly DataContext _context;
        
        public LikeRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Like> DoesLike(int likerId, int likeeId)
        {
            return await _context.Likes.FirstOrDefaultAsync(l =>
                l.LikerId == likerId && l.LikeeId == likeeId);
        }
    }
}