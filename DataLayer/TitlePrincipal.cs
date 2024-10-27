using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer
{
    public class TitlePrincipal
    {
        [StringLength(10)]
        public string Tconst { get; set; }
        public int Ordering { get; set; }   
        [StringLength(10)]
        public string Nconst { get; set; }
        public string Category { get; set; }
        public string Job { get; set; }
        public string Characters { get; set; }
        // Navigation Properties 
        public TitleBasic TitleBasic { get; set; }
        public NameBasic NameBasic { get; set; }
    }
}
