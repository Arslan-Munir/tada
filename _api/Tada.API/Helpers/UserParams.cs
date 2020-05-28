namespace Tada.API.Helpers
{
    public class UserParams
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
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;
        public string OrderBy { get; set; }   
    }
}