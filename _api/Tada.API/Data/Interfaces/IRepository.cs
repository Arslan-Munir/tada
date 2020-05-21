using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tada.API.Data.Interfaces
{
    public interface IRepository<T> where T: class
    {
        void Add(T entity);
        Task<T> Get(int id);
        Task<List<T>> GetAll();
        void Update(T entity);
        void Delete(T entity);
        Task<bool> SaveAll();
    }
}