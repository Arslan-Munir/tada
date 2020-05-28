using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tada.API.Data.Interfaces;
using Tada.API.Helpers;
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

        public async Task<PagedList<User>> GetAll(UserParams userParams)
        {
            var users = _context.Users.Include(x => x.Photos)
                .OrderByDescending(u=>u.LastActive).AsQueryable();
            users = users.Where(u => u.Id != userParams.UserId);
            users = users.Where(u => u.Gender == userParams.Gender);

            if(userParams.Likers)
            {
                var userLikers = await UserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }
            
            if(userParams.Likees)
            {
                var userLikees = await UserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }

            if(userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob); 
            }

            if(!string.IsNullOrWhiteSpace(userParams.OrderBy)){
                switch(userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }
            return await PagedList<User>.CreateAsync(users, userParams.CurrentPage, userParams.ItemsPerPage);
        }

        private async Task<IEnumerable<int>> UserLikes(int id, bool likers)
        {
            var user = await _context.Users
                .Include(x => x.Likers)
                .Include(x => x.Likees)
                .FirstOrDefaultAsync(u => u.Id == id);

            if(likers)
                return user.Likers.Where(u => u.LikeeId == id).Select(i => i.LikerId);
            else
                return user.Likees.Where(u => u.LikerId == id).Select(i=>i.LikeeId);
        }
    }
}