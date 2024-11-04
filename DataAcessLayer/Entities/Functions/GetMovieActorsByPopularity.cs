using DataAcessLayer.Entities.Movies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*GetMovieActorsByPopularity we using for function
"get_movie_actors_by_popularity"("p_tconst" bpchar)
  RETURNS TABLE("nconst" bpchar, "primaryname" varchar, "weighted_rating" numeric)*/
namespace DataAcessLayer.Entities.Functions;

public class GetMovieActorsByPopularity
{
    [StringLength(10)]
    public string Nconst { get; set; }
    public double WeightedRating { get; set; }
    public string PrimaryName { get; set; }
    // Navigation Properties 
    //public NameBasic NameBasic { get; set; }
}
