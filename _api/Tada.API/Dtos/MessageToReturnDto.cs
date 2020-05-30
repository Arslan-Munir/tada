using System;

namespace Tada.API.Dtos
{
    public class MessageToReturnDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public bool SenderDeleted { get; set; }
        public bool ReceiverDeleted { get; set; }
        public string SenderKnownAs { get; set; }
        public string SenderPhotoUrl { get; set; }
        public string ReceiverKnownAs { get; set; }
        public string ReceiverPhotoUrl { get; set; }
    }
}