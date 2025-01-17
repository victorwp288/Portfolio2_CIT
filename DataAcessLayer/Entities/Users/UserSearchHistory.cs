﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataAcessLayer.Entities.Users
{
    public class UserSearchHistory
    {
        public int UserId { get; set; }
        public string? SearchQuery { get; set; }
        public DateTime SearchDate { get; set; }
        // Navigation Properties 
        public User User { get; set; }

    }
}
