using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer
{
    public class TitleCrew
    {
        [Key]
        [StringLength(10)]
        public string Tconst { get; set; }
        public string? Directors { get; set; }
        public string? Writers { get; set; }
        // Navigation Properties 
        public TitleBasic TitleBasic { get; set; }
    }
}
