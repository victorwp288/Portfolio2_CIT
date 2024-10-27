using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataLayer
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

    }
}
