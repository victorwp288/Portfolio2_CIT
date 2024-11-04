namespace WebServiceLayer.Models.Movies
{
    public class Movie
    {
        public string? Url { get; set; }

        public string TConst { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public int? RunTimeMinutes { get; set; }
        public List<string> Genres { get; set; }
        public decimal? AverageRating { get; set; }
        public int? NumVotes { get; set; }
    }
}
