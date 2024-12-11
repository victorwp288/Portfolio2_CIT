using DataAcessLayer.Entities.Movies;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebServiceLayer.Models.Persons;

public class PersonDetailsModel
{
    
    //public string? Nconst { get; set; }
    public string? PrimaryName { get; set; }
    public string? BirthYear { get; set; }
    public string? DeathYear { get; set; }

    public ICollection<PersonProfessionModel>? PersonProfessions { get; set; }
    public ICollection<PersonKnownTitleModel>? PersonKnownTitles { get; set; }
    public ICollection<TitlePrincipalModel>? TitlePrincipals { get; set; }
    public NameRatingModel? NameRatings { get; set; }
}

public class PersonProfessionModel
{
    //public string? Nconst { get; set; }
    public string? Profession { get; set; }
}
public class PersonKnownTitleModel
{
    public string? Tconst { get; set; }
    //public string? Nconst { get; set; }
}
public class TitlePrincipalModel
{
    public string? Tconst { get; set; }
    public int? Ordering { get; set; }
    //public string? Nconst { get; set; }
    public string? Category { get; set; }
    public string? Job { get; set; }
    public string? Characters { get; set; }
}
public class NameRatingModel
{
    //public string? Nconst { get; set; }
    public string? PrimaryName { get; set; }
    public decimal? WeightedRating { get; set; }
}