using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DataLayer;

namespace DataAcessLayer
{
    public class TitleRating
    {
        [Key]
        [StringLength(10)]
        public string Tconst { get; set; }
        public decimal AverageRating { get; set; }
        public int NumVotes { get; set; }
        // Navigation Properties 
        public TitleBasic TitleBasic { get; set; }
    }
}
