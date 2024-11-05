namespace WebServiceLayer.Models.Movies
{
    public class RatingModel
    {
        public int Id { get; set; }

        public string TConst { get; set; }

        public int Rating { get; set; }

        public string? Review { get; set; }

        public DateTime ReviewDate { get; set; }


    }
}
