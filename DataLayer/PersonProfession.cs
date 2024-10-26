using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer
{
    public class PersonProfession
    {
        [StringLength(10)]
        public string Nconst { get; set; }
        public string Profession { get; set; }
        // Navigation Properties 
        public NameBasic NameBasic { get; set; }
    }
}
