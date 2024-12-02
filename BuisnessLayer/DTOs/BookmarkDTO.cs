using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.DTOs
{
    public class BookmarkDTO
    {
        public int UserId { get; set; }
        public string Tconst { get; set; }
        public string? Note { get; set; }
        public DateTime BookmarkDate { get; set; }
    }
}
