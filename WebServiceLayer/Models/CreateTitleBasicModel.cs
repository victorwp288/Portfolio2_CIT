using System.ComponentModel.DataAnnotations;

namespace WebServiceLayer.Models
{
    public class CreateTitleBasicModel
    {
        [MaxLength(10)]
        public string? Tconst { get; set; }

        public string? TitleType { get; set; }

        public string? PrimaryTitle { get; set; }
    }
}
