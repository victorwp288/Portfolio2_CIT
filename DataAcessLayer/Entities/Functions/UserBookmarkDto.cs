using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entities.Functions
{
    public class UserBookmarkDto
    {
        public int UserId { get; set; }
        public string Tconst { get; set; }
        public string Note { get; set; }
        public DateTime BookmarkDate { get; set; }
    }
}
