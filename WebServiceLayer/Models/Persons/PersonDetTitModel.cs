using BusinessLayer.DTOs;

namespace WebServiceLayer.Models.Persons
{
    public class PersonDetTitModel
    {
        public string NConst { get; set; }
        public string PrimaryName { get; set; }
        public string BirthYear { get; set; }
        public string DeathYear { get; set; }
        public List<string> Professions { get; set; }
        public List<PersonTitleModel> KnownForTitles { get; set; }
    }
    public class PersonTitleModel
    {
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
