using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Movies
{
    public class NameRating
    {
        [Key]
        [StringLength(10)] // Ensures the Nconst property is exactly 10 characters long
        public string Nconst { get; set; }

        [MaxLength(255)] // Sets the maximum length 
        public string PrimaryName { get; set; }

        public decimal WeightedRating { get; set; } // Useing decimal for NUMERIC data type

        // Navigation Properties 
        public NameBasic NameBasic { get; set; }
    }
}
