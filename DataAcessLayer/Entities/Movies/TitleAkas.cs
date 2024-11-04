using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entities.Movies
{
    public class TitleAkas
    {
        [StringLength(10)]
        public string Tconst { get; set; }//Converting titleid to Tconst
        public int Ordering { get; set; }
        public string? Title { get; set; }
        public string? Region { get; set; }
        public string? Language { get; set; }
        public string? Types { get; set; }
        public string? Attributes { get; set; }
        public bool IsOriginalTitle { get; set; }
        // Navigation Properties 
        public TitleBasic TitleBasic { get; set; }
    }
}
