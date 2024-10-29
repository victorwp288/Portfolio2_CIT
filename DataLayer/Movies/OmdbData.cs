﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Movies
{
    public class OmdbData
    {
        [Key]
        [StringLength(10)]
        public string Tconst { get; set; }
        public string? Episode { get; set; }
        public string? Awards { get; set; }
        public string? Plot { get; set; }
        public string? SeriesId { get; set; }
        public string? Rated { get; set; }
        public string? ImdbRating { get; set; }
        public string? Runtime { get; set; }
        public string? Language { get; set; }
        public string? Released { get; set; }
        public string? Response { get; set; }
        public string? Writer { get; set; }
        public string? Genre { get; set; }
        public string? Title { get; set; }
        public string? Country { get; set; }
        public string? Dvd { get; set; }
        public string? Production { get; set; }
        public string? Season { get; set; }
        public string? Type { get; set; }
        public string? Poster { get; set; }
        public string? Ratings { get; set; }
        public string? ImdbVotes { get; set; }
        public string? BoxOffice { get; set; }
        public string? Actors { get; set; }
        public string? Director { get; set; }
        public string? Year { get; set; }
        public string? Website { get; set; }
        public string? Metascore { get; set; }
        public string? TotalSeasons { get; set; }
        [NotMapped]
        public string? PlotTsvector { get; set; } // This is not mapped to the database

        // Navigation Properties 
        public TitleBasic TitleBasic { get; set; }

    }
}