﻿namespace WebServiceLayer.Models.Users
{
    public class CreateBookmarkModel
    {
        public int UserId { get; set; }
        public string Tconst { get; set; }
        public string Note { get; set; }
        public DateTime BookmarkDate { get; set; }
    }
}
