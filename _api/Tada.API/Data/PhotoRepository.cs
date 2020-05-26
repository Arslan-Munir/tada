using Tada.API.Data.Interfaces;
using Tada.API.Models;

namespace Tada.API.Data
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        public PhotoRepository(DataContext context) : base(context)
        {
        }
    }
}