using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class UserRatingReview
    {
       
        [Key]
        [Column(Order = 0)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string Tconst { get; set; } // Assuming Tconst is a string in C#

        [Range(1, 10)] // Data annotation for rating range
        public int Rating { get; set; }

        public string Review { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ReviewDate { get; set; }

        // Navigation Properties (optional)
        public User User { get; set; }
        public TitleBasic TitleBasic { get; set; }
        
    }
}
