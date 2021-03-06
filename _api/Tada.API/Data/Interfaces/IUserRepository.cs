using System.Collections.Generic;
using System.Threading.Tasks;
using Tada.API.Helpers;
using Tada.API.Models;

namespace Tada.API.Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        new Task<User> Get(int id);
        Task<PagedList<User>> GetAll(UserParams userParams);
    }
}