using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entities.Users
{
    public class UserSearchHistory
    {
        public int UserId { get; set; }
        public string? SearchQuery { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime SearchDate { get; set; }
        // Navigation Properties 
        public User User { get; set; }

    }
}
