using DataLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataService;
internal class ImdbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserBookmark> UserBookmarks { get; set; }
    public DbSet<UserRatingReview> UserRatingReviews { get; set; }
    public DbSet<NameRating> NameRatings { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.UseNpgsql("host=localhost;db=imdb;uid=postgres;pwd=2409");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<User>().Property(x => x.UserId).HasColumnName("user_id");
        modelBuilder.Entity<User>().Property(x => x.Username).HasColumnName("username");
        modelBuilder.Entity<User>().Property(x => x.Email).HasColumnName("email");
        modelBuilder.Entity<User>().Property(x => x.PasswordHash).HasColumnName("password_hash");
        modelBuilder.Entity<User>().Property(x => x.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<User>().Property(x => x.Role).HasColumnName("role");

        modelBuilder.Entity<UserBookmark>().ToTable("user_bookmarks");
        modelBuilder.Entity<UserBookmark>().Property(x => x.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserBookmark>().Property(x => x.Tconst).HasColumnName("tconst");
        modelBuilder.Entity<UserBookmark>().HasKey(x => new { x.UserId, x.Tconst });
        modelBuilder.Entity<UserBookmark>().Property(x => x.Note).HasColumnName("note");
        modelBuilder.Entity<UserBookmark>().Property(x => x.BookmarkDate).HasColumnName("bookmark_date");

        modelBuilder.Entity<UserRatingReview>().ToTable("user_rating_review");
        modelBuilder.Entity<UserRatingReview>().Property(x => x.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserRatingReview>().Property(x => x.Tconst).HasColumnName("tconst");
        modelBuilder.Entity<UserRatingReview>().HasKey(x => new { x.UserId, x.Tconst });
        modelBuilder.Entity<UserRatingReview>().Property(x => x.Rating).HasColumnName("rating");
        modelBuilder.Entity<UserRatingReview>().Property(x => x.Review).HasColumnName("review");
        modelBuilder.Entity<UserRatingReview>().Property(x => x.ReviewDate).HasColumnName("review_date");

        modelBuilder.Entity<NameRating>().ToTable("name_rating");
        modelBuilder.Entity<NameRating>().Property(x =>x.Nconst).HasColumnName("nconst");
        modelBuilder.Entity<NameRating>().Property(x => x.PrimaryName).HasColumnName("primary_name");
        modelBuilder.Entity<NameRating>().Property(x => x.WeightedRating).HasColumnName("weighted_rating");
        modelBuilder.Entity<NameRating>().HasKey(x => x.Nconst);

    }
}
