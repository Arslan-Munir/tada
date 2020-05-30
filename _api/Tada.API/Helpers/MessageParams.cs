namespace Tada.API.Helpers
{
    public class MessageParams
    {
        private const int MaxPageSize = 50;

        public int CurrentPage { get; set; } = 1;
        private int itemsPerPage = 10;
        public int ItemsPerPage
        {
            get { return itemsPerPage; }
            set { itemsPerPage = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        public int UserId { get; set; }
        public string MessagesType { get; set; } = "unread";
    }
}