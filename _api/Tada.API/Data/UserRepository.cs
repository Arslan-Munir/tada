using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tada.API.Data.Interfaces;
using Tada.API.Models;

namespace Tada.API.Data
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context) : base(context) 
        {
            _context = context;
        }

        public new async Task<User> Get(int id)
        {
            return await _context.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.Id == id);
        }

        public new async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.Include(x => x.Photos).ToListAsync();
        }
    }
}