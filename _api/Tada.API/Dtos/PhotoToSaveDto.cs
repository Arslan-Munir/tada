using System;
using Microsoft.AspNetCore.Http;

namespace Tada.API.Dtos
{
    public class PhotoToSaveDto
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }

        public PhotoToSaveDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}