namespace BusinessLayer.DTOs
{
    using System.Collections.Generic;

    public class PersonDTO
    {
        public string NConst { get; set; }
        public string PrimaryName { get; set; }
        public string BirthYear { get; set; }
        public string DeathYear { get; set; }
        public List<string> Professions { get; set; }
        public List<TitleDTO> KnownForTitles { get; set; }
    }
}
