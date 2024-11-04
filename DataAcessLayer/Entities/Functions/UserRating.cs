using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entities.Functions
{
    public class UserRating
    {
        public int UserId { get; set; }
        public string Tconst { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
