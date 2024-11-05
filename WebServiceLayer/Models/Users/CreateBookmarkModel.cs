namespace WebServiceLayer.Models.Users
{
    public class CreateBookmarkModel
    {
        public int UserId { get; set; }
        public string TConst { get; set; }

        public string Note { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
