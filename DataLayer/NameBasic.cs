using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class NameBasic
    {
        [Key]
        [Column(Order = 1)]
        [MaxLength(10)]
        public string Nconst { get; set; }
        public string PrimaryName { get; set; }
        [MaxLength(4)] 
        public string? BirthYear { get; set; }
        [MaxLength(4)] 
        public string? DeathYear { get; set; }

    }
}
