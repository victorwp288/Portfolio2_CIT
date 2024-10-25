using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class UserBookmark
    {
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string Tconst { get; set; } // Assuming Tconst is a string in C#

        public string Note { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime BookmarkDate { get; set; }

        // Navigation Properties (optional, for relationships with other entities)
        public User User { get; set; }
        public TitleBasic TitleBasic { get; set; }
    }
}
