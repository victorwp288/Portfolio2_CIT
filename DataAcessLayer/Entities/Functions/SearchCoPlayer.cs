using DataAcessLayer.Entities.Movies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*SearchCoPlayers we using for function
"search_co_players"("p_search_name" varchar)
  RETURNS TABLE("nconst" bpchar, "primaryname" varchar, "frequency" int4)*/

namespace DataAcessLayer.Entities.Functions;

public class SearchCoPlayer
{
    [StringLength(10)]
    public string Nconst { get; set; }
    public int Frequency { get; set; }
    public string PrimaryName { get; set; }
    // Navigation Properties 
    public NameBasic NameBasic { get; set; }
}
