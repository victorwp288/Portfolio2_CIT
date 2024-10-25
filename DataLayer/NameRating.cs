using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class NameRating
    {
        [Key]
        [StringLength(10)] // Ensures the Nconst property is exactly 10 characters long
        public string Nconst { get; set; }

        [MaxLength(255)] // Sets the maximum length of the primary name
        public string PrimaryName { get; set; }

        public decimal WeightedRating { get; set; } // Use decimal for NUMERIC data type

        // Navigation Properties (optional, for relationships with other entities)
        public NameBasic NameBasic { get; set; }
    }
}
