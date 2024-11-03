using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAcessLayer.Entities.Movies
{
    public class TitleBasic
    {
        [Key]
        [Column(Order = 1)]
        [MaxLength(10)]
        public string Tconst { get; set; }
        public string TitleType { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        [MaxLength(4)]
        public string? StartYear { get; set; }
        [MaxLength(4)]
        public string? EndYear { get; set; }
        public int? RunTimeMinutes { get; set; }

        public ICollection<PersonKnownTitle> PersonKnownTitles { get; set; }
        public TitleRating TitleRating { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }

    }
}
