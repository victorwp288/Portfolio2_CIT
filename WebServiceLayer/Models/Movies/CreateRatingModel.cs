namespace WebServiceLayer.Models.Movies
{
    public class CreateRatingModel
    {
        //public int UserId { get; set; }
        //public string TConst { get; set; }
        public int Rating { get; set; }
        public string? Review { get; set; }
        public System.DateTime ReviewDate { get; set; }
    }
}
