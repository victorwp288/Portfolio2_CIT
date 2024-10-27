using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*BestMatchQuery we using for function
"best_match_query"("w1" text, "w2" text, "w3" text)
  RETURNS TABLE("tconst" bpchar, "rank" int8, "primarytitle" text)*/
namespace DataAccessLayerFunction;

public class BestMatchQuery
{
    [StringLength(10)]
    public string Tconst { get; set; }
    public string PrimaryTitle { get; set; }
    public int Rank { get; set; }
    // Navigation Properties 
    public TitleBasic TitleBasic { get; set; }
}
