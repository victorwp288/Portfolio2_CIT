using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Movies
{
    public class MovieGenre
    {
        [MaxLength(10)]
        public string Tconst { get; set; }
        public string Genre { get; set; }

        // Navigation Properties
        public TitleBasic TitleBasic { get; set; }
    }
}
