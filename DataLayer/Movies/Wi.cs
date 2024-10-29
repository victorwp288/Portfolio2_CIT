using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Movies
{
    public class Wi
    {
        [MaxLength(10)]
        public string Tconst { get; set; }
        public string Word { get; set; }
        public char Field { get; set; }
        public string? Lexeme { get; set; }
        // Navigation Properties 
        public TitleBasic TitleBasic { get; set; }
    }
}
