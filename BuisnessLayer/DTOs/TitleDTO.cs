﻿namespace BusinessLayer.DTOs
{
    using System.Collections.Generic;

    public class TitleDTO
    {
        public string TConst { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public int? RunTimeMinutes { get; set; }
        public List<string> Genres { get; set; }
        public decimal? AverageRating { get; set; }
        public int? NumVotes { get; set; }
    }
}
