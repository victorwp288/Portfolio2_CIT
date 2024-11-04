namespace WebServiceLayer.Models.Movies
{
    public class UserRating
    {
        public string? Url { get; set; }
        public int UserId { get; set; }
        public string TConst { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public System.DateTime ReviewDate { get; set; }
    }
}
