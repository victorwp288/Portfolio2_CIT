using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entities.Movies
{
    public class PersonKnownTitle
    {
        [StringLength(10)]
        public string Tconst { get; set; }
        [StringLength(10)]
        public string Nconst { get; set; }
        // Navigation Properties 
        public TitleBasic TitleBasic { get; set; }
        public NameBasic NameBasic { get; set; }
    }
}
