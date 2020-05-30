using System;

namespace Tada.API.Dtos
{
    public class MessageToCreateDto
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime SentDate { get; set; }
        public string Content { get; set; }

        public MessageToCreateDto()
        {
            SentDate = DateTime.Now;   
        }
    }
}