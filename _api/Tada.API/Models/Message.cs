using System;

namespace Tada.API.Models
{
    public class Message
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
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}