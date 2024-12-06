using DataAcessLayer.Entities.Functions;
using DataAcessLayer.Entities.Movies;
using DataAcessLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace DataAcessLayer.Context;
public class ImdbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ImdbContext()
    {
    }

    public ImdbContext(DbContextOptions<ImdbContext> options, IConfiguration configuration)
    : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserBookmark> UserBookmarks { get; set; }
    public DbSet<UserRatingReview> UserRatingReviews { get; set; }
    public DbSet<NameRating> NameRatings { get; set; }
    public DbSet<TitleBasic> TitleBasics { get; set; }
    public DbSet<NameBasic> NameBasics { get; set; }
    public DbSet<UserSearchHistory> UserSearchHistories { get; set; }
    public DbSet<TitleRating> TitleRatings { get; set; }
    public DbSet<Wi> Wis { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<OmdbData> OmdbDatas { get; set; }
    public DbSet<PersonKnownTitle> PersonKnownTitles { get; set; }
    public DbSet<PersonProfession> PersonProfessions { get; set; }
    public DbSet<TitleAkas> TitleAkas { get; set; }
    public DbSet<TitleCrew> TitleCrews { get; set; }
    public DbSet<TitleEpisode> TitleEpisodes { get; set; }
    public DbSet<TitlePrincipal> TitlePrincipals { get; set; }
    //functions
    public DbSet<TconstAndPrimaryTitle> TconstAndPrimaryTitles { get; set; }
    public DbSet<WordAndFrequency> WordAndFrequencies { get; set; }
    public DbSet<BestMatchQuery> BestMatchQueries { get; set; }
    public DbSet<GetMovieActorsByPopularity> GetMovieActorsByPopularity { get; set; }
    public DbSet<SearchCoPlayer> SearchCoPlayers { get; set; }
    public DbSet<SearchName> SearchNames { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            //optionsBuilder.UseNpgsql("host=localhost;db=imdb;uid=postgres;pwd=Hejmed12!");

            optionsBuilder.UseNpgsql("host=localhost;db=imdb;uid=postgres;pwd=2409");
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Tables
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<User>().Property(x => x.UserId).HasColumnName("user_id");
        modelBuilder.Entity<User>().Property(x => x.Username).HasColumnName("username");
        modelBuilder.Entity<User>().Property(x => x.Email).HasColumnName("email");
        modelBuilder.Entity<User>().Property(x => x.PasswordHash).HasColumnName("password_hash");
        modelBuilder.Entity<User>().Property(x => x.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<User>().Property(x => x.Role)
            .HasColumnName("role")
            .HasColumnType("user_role");

        modelBuilder.Entity<UserBookmark>().ToTable("user_bookmarks");
        modelBuilder.Entity<UserBookmark>().Property(x => x.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserBookmark>().Property(x => x.Tconst).HasColumnName("tconst");
        modelBuilder.Entity<UserBookmark>().HasKey(x => new { x.UserId, x.Tconst });
        modelBuilder.Entity<UserBookmark>().Property(x => x.Note).HasColumnName("note");
        modelBuilder.Entity<UserBookmark>().Property(x => x.BookmarkDate).HasColumnName("bookmark_date");

        modelBuilder.Entity<UserRatingReview>().ToTable("user_ratings_reviews");
        modelBuilder.Entity<UserRatingReview>().Property(x => x.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserRatingReview>().Property(x => x.Tconst).HasColumnName("tconst");
        modelBuilder.Entity<UserRatingReview>().HasKey(x => new { x.UserId, x.Tconst });
        modelBuilder.Entity<UserRatingReview>().Property(x => x.Rating).HasColumnName("rating");
        modelBuilder.Entity<UserRatingReview>().Property(x => x.Review).HasColumnName("review");
        modelBuilder.Entity<UserRatingReview>().Property(x => x.ReviewDate).HasColumnName("review_date");

        modelBuilder.Entity<NameRating>().ToTable("name_ratings");
        modelBuilder.Entity<NameRating>().Property(x => x.Nconst).HasColumnName("nconst");
        modelBuilder.Entity<NameRating>().Property(x => x.PrimaryName).HasColumnName("primaryname");
        modelBuilder.Entity<NameRating>().Property(x => x.WeightedRating).HasColumnName("weighted_rating");


        modelBuilder.Entity<TitleBasic>().ToTable("title_basics");
        modelBuilder.Entity<TitleBasic>().Property(x => x.Tconst).HasColumnName("tconst");
        modelBuilder.Entity<TitleBasic>().Property(x => x.TitleType).HasColumnName("titletype");
        modelBuilder.Entity<TitleBasic>().Property(x => x.PrimaryTitle).HasColumnName("primarytitle");
        modelBuilder.Entity<TitleBasic>().Property(x => x.OriginalTitle).HasColumnName("originaltitle");
        modelBuilder.Entity<TitleBasic>().Property(x => x.IsAdult).HasColumnName("isadult");
        modelBuilder.Entity<TitleBasic>().Property(x => x.StartYear).HasColumnName("startyear");
        modelBuilder.Entity<TitleBasic>().Property(x => x.EndYear).HasColumnName("endyear");
        modelBuilder.Entity<TitleBasic>().Property(x => x.RunTimeMinutes).HasColumnName("runtimeminutes");

        modelBuilder.Entity<NameBasic>().ToTable("name_basics");
        modelBuilder.Entity<NameBasic>().Property(x => x.Nconst).HasColumnName("nconst");
        modelBuilder.Entity<NameBasic>().Property(x => x.PrimaryName).HasColumnName("primaryname");
        modelBuilder.Entity<NameBasic>().Property(x => x.BirthYear).HasColumnName("birthyear");
        modelBuilder.Entity<NameBasic>().Property(x => x.DeathYear).HasColumnName("deathyear");

        modelBuilder.Entity<UserSearchHistory>().ToTable("user_search_history");
        modelBuilder.Entity<UserSearchHistory>().Property(x => x.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserSearchHistory>().Property(x => x.SearchQuery).HasColumnName("search_query");
        modelBuilder.Entity<UserSearchHistory>().Property(x => x.SearchDate).HasColumnName("search_date");
        modelBuilder.Entity<UserSearchHistory>().HasKey(x => new { x.UserId, x.SearchDate });

        modelBuilder.Entity<Wi>().ToTable("wi");
        modelBuilder.Entity<Wi>().Property(x => x.Word).HasColumnName("word");
        modelBuilder.Entity<Wi>().Property(x => x.Tconst).HasColumnName("tconst");
        modelBuilder.Entity<Wi>().Property(x => x.Field).HasColumnName("field");
        modelBuilder.Entity<Wi>().HasKey(x => new { x.Word, x.Tconst, x.Field });
        modelBuilder.Entity<Wi>().Property(x => x.Lexeme).HasColumnName("lexeme");

        modelBuilder.Entity<TitleRating>().ToTable("title_ratings");
        modelBuilder.Entity<TitleRating>().Property(x => x.Tconst).HasColumnName("tconst");
        modelBuilder.Entity<TitleRating>().Property(x => x.AverageRating).HasColumnName("averagerating");
        modelBuilder.Entity<TitleRating>().Property(x => x.NumVotes).HasColumnName("numvotes");

        modelBuilder.Entity<MovieGenre>().ToTable("movie_genres");
        modelBuilder.Entity<MovieGenre>().Property(x => x.Tconst).HasColumnName("tconst");
        modelBuilder.Entity<MovieGenre>().Property(x => x.Genre).HasColumnName("genre");
        modelBuilder.Entity<MovieGenre>().HasKey(x => new { x.Genre, x.Tconst });

        modelBuilder.Entity<OmdbData>().ToTable("omdb_data");
        modelBuilder.Entity<OmdbData>().Property(o => o.Tconst).HasMaxLength(10).HasColumnName("tconst");
        modelBuilder.Entity<OmdbData>().Property(o => o.Episode).HasColumnName("episode");
        modelBuilder.Entity<OmdbData>().Property(o => o.Awards).HasColumnName("awards");
        modelBuilder.Entity<OmdbData>().Property(o => o.Plot).HasColumnName("plot");
        modelBuilder.Entity<OmdbData>().Property(o => o.SeriesId).HasColumnName("seriesid");
        modelBuilder.Entity<OmdbData>().Property(o => o.Rated).HasColumnName("rating");
        modelBuilder.Entity<OmdbData>().Property(o => o.ImdbRating).HasColumnName("imdbrating");
        modelBuilder.Entity<OmdbData>().Property(o => o.Runtime).HasColumnName("runtime");
        modelBuilder.Entity<OmdbData>().Property(o => o.Language).HasColumnName("language");
        modelBuilder.Entity<OmdbData>().Property(o => o.Released).HasColumnName("released");
        modelBuilder.Entity<OmdbData>().Property(o => o.Response).HasColumnName("response");
        modelBuilder.Entity<OmdbData>().Property(o => o.Genre).HasColumnName("gener");
        modelBuilder.Entity<OmdbData>().Property(o => o.Title).HasColumnName("title");
        modelBuilder.Entity<OmdbData>().Property(o => o.Country).HasColumnName("country");
        modelBuilder.Entity<OmdbData>().Property(o => o.Dvd).HasColumnName("dvd");
        modelBuilder.Entity<OmdbData>().Property(o => o.Production).HasColumnName("production");
        modelBuilder.Entity<OmdbData>().Property(o => o.Season).HasColumnName("season");
        modelBuilder.Entity<OmdbData>().Property(o => o.Type).HasColumnName("type");
        modelBuilder.Entity<OmdbData>().Property(o => o.Poster).HasColumnName("poster");
        modelBuilder.Entity<OmdbData>().Property(o => o.Ratings).HasColumnName("ratings");
        modelBuilder.Entity<OmdbData>().Property(o => o.ImdbVotes).HasColumnName("imdbvotes");
        modelBuilder.Entity<OmdbData>().Property(o => o.BoxOffice).HasColumnName("boxoffice");
        modelBuilder.Entity<OmdbData>().Property(o => o.Actors).HasColumnName("actors");
        modelBuilder.Entity<OmdbData>().Property(o => o.Year).HasColumnName("year");
        modelBuilder.Entity<OmdbData>().Property(o => o.Website).HasColumnName("website");
        modelBuilder.Entity<OmdbData>().Property(o => o.Metascore).HasColumnName("metascore");
        modelBuilder.Entity<OmdbData>().Property(o => o.TotalSeasons).HasColumnName("totalseasons");
        // Ignore the PlotTsvector property as it's not mapped to the database
        modelBuilder.Entity<OmdbData>().Ignore(o => o.PlotTsvector);

        modelBuilder.Entity<PersonKnownTitle>().ToTable("person_known_titles");
        modelBuilder.Entity<PersonKnownTitle>().Property(x => x.Tconst).HasColumnName("tconst");
        modelBuilder.Entity<PersonKnownTitle>().Property(x => x.Nconst).HasColumnName("nconst");
        modelBuilder.Entity<PersonKnownTitle>().HasKey(x => new { x.Nconst, x.Tconst });

        modelBuilder.Entity<PersonProfession>().ToTable("person_professions");
        modelBuilder.Entity<PersonProfession>().Property(x => x.Nconst).HasColumnName("nconst");
        modelBuilder.Entity<PersonProfession>().Property(x => x.Profession).HasColumnName("profession");
        modelBuilder.Entity<PersonProfession>().HasKey(x => new { x.Nconst, x.Profession });

        modelBuilder.Entity<TitleAkas>().ToTable("title_akas");
        modelBuilder.Entity<TitleAkas>().Property(x => x.Tconst).HasColumnName("titleid");
        modelBuilder.Entity<TitleAkas>().Property(x => x.Ordering).HasColumnName("ordering");
        modelBuilder.Entity<TitleAkas>().HasKey(x => new { x.Ordering, x.Tconst });
        modelBuilder.Entity<TitleAkas>().Property(x => x.Title).HasColumnName("title");
        modelBuilder.Entity<TitleAkas>().Property(x => x.Region).HasColumnName("region");
        modelBuilder.Entity<TitleAkas>().Property(x => x.Language).HasColumnName("language");
        modelBuilder.Entity<TitleAkas>().Property(x => x.Types).HasColumnName("types");
        modelBuilder.Entity<TitleAkas>().Property(x => x.Attributes).HasColumnName("attributes");
        modelBuilder.Entity<TitleAkas>().Property(x => x.IsOriginalTitle).HasColumnName("isoriginaltitle");

        modelBuilder.Entity<TitleCrew>().ToTable("title_crew");
        modelBuilder.Entity<TitleCrew>().Property(x => x.Tconst).HasColumnName("tconst");
        modelBuilder.Entity<TitleCrew>().Property(x => x.Directors).HasColumnName("directors");
        modelBuilder.Entity<TitleCrew>().Property(x => x.Writers).HasColumnName("writers");

        modelBuilder.Entity<TitleEpisode>().ToTable("title_episode");
        modelBuilder.Entity<TitleEpisode>().Property(x => x.Tconst).HasColumnName("tconst");
        modelBuilder.Entity<TitleEpisode>().Property(x => x.ParentTconst).HasColumnName("parenttconst");
        modelBuilder.Entity<TitleEpisode>().Property(x => x.SeasonNumber).HasColumnName("seasonnumber");
        modelBuilder.Entity<TitleEpisode>().Property(x => x.EpisodeNumber).HasColumnName("episodenumber");

        modelBuilder.Entity<TitlePrincipal>().ToTable("title_principals");
        modelBuilder.Entity<TitlePrincipal>().Property(x => x.Tconst).HasColumnName("tconst");
        modelBuilder.Entity<TitlePrincipal>().Property(x => x.Ordering).HasColumnName("ordering");
        modelBuilder.Entity<TitlePrincipal>().HasKey(x => new { x.Ordering, x.Tconst });
        modelBuilder.Entity<TitlePrincipal>().Property(x => x.Nconst).HasColumnName("nconst");
        modelBuilder.Entity<TitlePrincipal>().Property(x => x.Category).HasColumnName("category");
        modelBuilder.Entity<TitlePrincipal>().Property(x => x.Job).HasColumnName("job");
        modelBuilder.Entity<TitlePrincipal>().Property(x => x.Characters).HasColumnName("characters");

        //functions
        //TconstAndPrimaryTitle
        modelBuilder.Entity<TconstAndPrimaryTitle>().HasNoKey();
        modelBuilder.Entity<TconstAndPrimaryTitle>().Property(x => x.Tconst).HasMaxLength(10).HasColumnName("tconst");
        modelBuilder.Entity<TconstAndPrimaryTitle>().Property(x => x.PrimaryTitle).HasColumnName("primarytitle");

        //WordAndFrequency
        modelBuilder.Entity<WordAndFrequency>().HasNoKey();
        modelBuilder.Entity<WordAndFrequency>().Property(x => x.Word).HasColumnName("word");
        modelBuilder.Entity<WordAndFrequency>().Property(x => x.Frequency).HasColumnName("frequency");

        //BestMatchQuery
        modelBuilder.Entity<BestMatchQuery>().HasNoKey();
        modelBuilder.Entity<BestMatchQuery>().Property(x => x.Tconst).HasMaxLength(10).HasColumnName("tconst");
        modelBuilder.Entity<BestMatchQuery>().Property(x => x.PrimaryTitle).HasColumnName("primarytitle");
        modelBuilder.Entity<BestMatchQuery>().Property(x => x.Rank).HasColumnName("rank");

        //GetMovieActorsByPopularity
        modelBuilder.Entity<GetMovieActorsByPopularity>().HasNoKey();
        modelBuilder.Entity<GetMovieActorsByPopularity>().Property(x => x.Nconst).HasColumnName("nconst");
        modelBuilder.Entity<GetMovieActorsByPopularity>().Property(x => x.PrimaryName).HasColumnName("primaryname");
        modelBuilder.Entity<GetMovieActorsByPopularity>().Property(x => x.WeightedRating).HasColumnName("weighted_rating");
        //SearchCoPlayer
        modelBuilder.Entity<SearchCoPlayer>().HasNoKey();
        modelBuilder.Entity<SearchCoPlayer>().Property(x => x.Nconst).HasColumnName("nconst");
        modelBuilder.Entity<SearchCoPlayer>().Property(x => x.PrimaryName).HasColumnName("primaryname");
        modelBuilder.Entity<SearchCoPlayer>().Property(x => x.Frequency).HasColumnName("frequency");

        //SearchName
        modelBuilder.Entity<SearchName>().HasNoKey();
        modelBuilder.Entity<SearchName>().Property(x => x.Tconst).HasMaxLength(10).HasColumnName("tconst");
        modelBuilder.Entity<SearchName>().Property(x => x.PrimaryTitle).HasColumnName("primarytitle");
        modelBuilder.Entity<SearchName>().Property(x => x.Nconst).HasColumnName("nconst");
        modelBuilder.Entity<SearchName>().Property(x => x.PrimaryName).HasColumnName("primaryname");

        // Configure one-to-one relationship between TitleBasic and TitleRating
        modelBuilder.Entity<TitleBasic>()
            .HasOne(tb => tb.TitleRating)
            .WithOne(tr => tr.TitleBasic)
            .HasForeignKey<TitleRating>(tr => tr.Tconst);

        // Configure one-to-many relationship between TitleBasic and MovieGenre
        modelBuilder.Entity<TitleBasic>()
            .HasMany(tb => tb.MovieGenres)
            .WithOne(mg => mg.TitleBasic)
            .HasForeignKey(mg => mg.Tconst);

        // Configure one-to-many relationship between TitleBasic and MovieGenre
        modelBuilder.Entity<TitleBasic>()
            .HasMany(tb => tb.PersonKnownTitles)
            .WithOne(pk => pk.TitleBasic)
            .HasForeignKey(pk => pk.Tconst);

        // Configure one-to-one relationship between NameBasic and NameRating
        modelBuilder.Entity<NameBasic>()
            .HasOne(nb => nb.NameRatings)
            .WithOne(nr => nr.NameBasic)
            .HasForeignKey<NameRating>(nr => nr.Nconst);

        // Configure one-to-many relationship between NameBasic and TitlePrincipal
        modelBuilder.Entity<NameBasic>()
            .HasMany(nb => nb.TitlePrincipals)
            .WithOne(tp => tp.NameBasic)
            .HasForeignKey(tp => tp.Nconst);

        // Configure one-to-many relationship between NameBasic and PersonProfession
        modelBuilder.Entity<NameBasic>()
            .HasMany(nb => nb.PersonProfessions)
            .WithOne(pp => pp.NameBasic)
            .HasForeignKey(pp => pp.Nconst);

        // Configure one-to-many relationship between NameBasic and PersonKnownTitle
        modelBuilder.Entity<NameBasic>()
            .HasMany(nb => nb.PersonKnownTitles)
            .WithOne(pk => pk.NameBasic)
            .HasForeignKey(pk => pk.Nconst);

        // Add enum mapping
        modelBuilder.HasPostgresEnum<UserRole>();

        // Add this configuration for UserRatingReview
        modelBuilder.Entity<UserRatingReview>()
            .HasOne(ur => ur.TitleBasic)
            .WithMany()
            .HasForeignKey(ur => ur.Tconst);

        modelBuilder.Entity<UserRatingReview>()
            .HasOne(ur => ur.User)
            .WithMany()
            .HasForeignKey(ur => ur.UserId);

        // Add this configuration for UserBookmark
        modelBuilder.Entity<UserBookmark>()
            .HasOne(ub => ub.TitleBasic)
            .WithMany()
            .HasForeignKey(ub => ub.Tconst);

        modelBuilder.Entity<UserBookmark>()
            .HasOne(ub => ub.User)
            .WithMany()
            .HasForeignKey(ub => ub.UserId);
    }
}
