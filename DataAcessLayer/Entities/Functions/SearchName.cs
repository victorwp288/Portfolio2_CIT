using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Entities.Movies;

/*SearchNames we using for function
"search_names"("p_search_text" varchar)
  RETURNS TABLE("tconst" bpchar, "primarytitle" text, "nconst" bpchar, "primaryname" varchar)*/

namespace DataAcessLayer.Entities.Functions;

public class SearchName
{
    [StringLength(10)]
    public string Tconst { get; set; }
    [StringLength(10)]
    public string Nconst { get; set; }
    public string PrimaryTitle { get; set; }
    public string PrimaryName { get; set; }
    // Navigation Properties 
    public TitleBasic TitleBasic { get; set; }
    public NameBasic NameBasic { get; set; }
}
