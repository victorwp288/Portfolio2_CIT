
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

/*
TconstAndPrimaryTitle is using for functions
"exact_match_query"("w1" text, "w2" text, "w3" text)
  RETURNS TABLE("tconst" bpchar, "primarytitle" text)
"other_movies_like_this"("p_search_name" varchar)
  RETURNS TABLE("tconst" bpchar, "primarytitle" text)
"search_movies"("p_search_text" varchar)
  RETURNS TABLE("tconst" bpchar, "primarytitle" text)
"structured_search"("p_title" text, "p_plot" text, "p_actor" text)
  RETURNS TABLE("tconst" bpchar, "primarytitle" text)*/



namespace DataAcessLayerFunction;

public class TconstAndPrimaryTitle
{
    [MaxLength(10)]
    public string Tconst { get; set; }
    public string PrimaryTitle { get; set; }

    // Navigation Properties
    public TitleBasic TitleBasic { get; set; }

    
}

