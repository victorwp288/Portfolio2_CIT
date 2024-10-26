using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer
{
    public class TitleEpisode
    {
        [Key]
        [StringLength(10)]
        public string Tconst { get; set; }
        [StringLength(10)]
        public string ParentTconst { get; set; }
        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }
        // Navigation Properties 
        public TitleBasic TitleBasic { get; set; }
    }
}
