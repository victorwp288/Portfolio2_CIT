using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Entities.Movies;

namespace DataAcessLayer.Entities.Users
{
    public class UserRatingReview
    {


        public int UserId { get; set; }

        [StringLength(10)]
        public string Tconst { get; set; }

        [Range(1, 10)] // Data annotation for rating range
        public int Rating { get; set; }

        public string? Review { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ReviewDate { get; set; }

        // Navigation Properties 
        public User User { get; set; }
        public TitleBasic TitleBasic { get; set; }


    }
}
