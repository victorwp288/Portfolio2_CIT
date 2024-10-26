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

        [StringLength(10)]
        public string Tconst { get; set; } 

        public string? Note { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime BookmarkDate { get; set; }

        // Navigation Properties 
        public User User { get; set; }
        public TitleBasic TitleBasic { get; set; }
    }
}
