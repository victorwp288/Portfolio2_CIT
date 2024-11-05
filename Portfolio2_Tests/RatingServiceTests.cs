using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataAcessLayer.Context;
using DataAcessLayer.Entities.Movies;
using DataAcessLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Portfolio2_Tests
{
    public class RatingServiceTests : IDisposable
    {
        private readonly IRatingService _ratingService;
        private readonly TestDbContext _context;

        public RatingServiceTests()
        {
            var options = new DbContextOptionsBuilder<ImdbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            // Seed the database with initial data
            SeedDatabase();

            _ratingService = new RatingService(_context);
        }

        private void SeedDatabase()
        {
            // Add a user
            _context.Users.Add(new User
            {
                UserId = 1,
                Email = "user@example.com",
                Username = "User",
                PasswordHash = "password123",
                CreatedAt = DateTime.UtcNow,
                Role = UserRole.user
            });

            // Add a title
            _context.TitleBasics.Add(new TitleBasic
            {
                Tconst = "tt0000001",
                TitleType = "movie", // Added TitleType
                PrimaryTitle = "Test Movie",
                OriginalTitle = "Test Movie",
                IsAdult = false,
                StartYear = "2020",
                RunTimeMinutes = 120 // Optional, but good to include if applicable
            });

            _context.SaveChanges();
        }


        [Fact]
        public async Task SubmitUserRatingAsync_Should_Add_New_Rating()
        {
            // Arrange
            var ratingDto = new UserRatingDTO
            {
                UserId = 1,
                TConst = "tt0000001",
                Rating = 9,
                Review = "Great movie!"
            };

            // Act
            await _ratingService.SubmitUserRatingAsync(ratingDto);

            // Assert
            var userRating = await _context.UserRatingReviews
                                       .SingleOrDefaultAsync(r => r.UserId == 1 && r.Tconst == "tt0000001");
            Assert.NotNull(userRating);
            Assert.Equal(9, userRating.Rating);
            Assert.Equal("Great movie!", userRating.Review);

            // Check TitleRating is updated
            var titleRating = await _context.TitleRatings.FindAsync("tt0000001");
            Assert.NotNull(titleRating);
            Assert.Equal(9m, titleRating.AverageRating);
            Assert.Equal(1, titleRating.NumVotes);
        }

        [Fact]
        public async Task SubmitUserRatingAsync_Should_Update_Existing_Rating()
        {
            // Arrange
            // First, submit an initial rating
            var initialRatingDto = new UserRatingDTO
            {
                UserId = 1,
                TConst = "tt0000001",
                Rating = 8,
                Review = "Good movie."
            };
            await _ratingService.SubmitUserRatingAsync(initialRatingDto);

            // Act
            // Update the rating
            var updatedRatingDto = new UserRatingDTO
            {
                UserId = 1,
                TConst = "tt0000001",
                Rating = 10,
                Review = "Amazing movie!"
            };
            await _ratingService.SubmitUserRatingAsync(updatedRatingDto);

            // Assert
            var userRating = await _context.UserRatingReviews
                                       .SingleOrDefaultAsync(r => r.UserId == 1 && r.Tconst == "tt0000001");
            Assert.NotNull(userRating);
            Assert.Equal(10, userRating.Rating);
            Assert.Equal("Amazing movie!", userRating.Review);

            // Check TitleRating is updated
            var titleRating = await _context.TitleRatings.FindAsync("tt0000001");
            Assert.NotNull(titleRating);
            Assert.Equal(10m, titleRating.AverageRating);
            Assert.Equal(1, titleRating.NumVotes);
        }

        [Fact]
        public async Task GetUserRatingAsync_Should_Return_User_Rating()
        {
            // Arrange
            var ratingDto = new UserRatingDTO
            {
                UserId = 1,
                TConst = "tt0000001",
                Rating = 8,
                Review = "Good movie."
            };
            await _ratingService.SubmitUserRatingAsync(ratingDto);

            // Act
            var result = await _ratingService.GetUserRatingAsync(1, "tt0000001");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.Rating);
            Assert.Equal("Good movie.", result.Review);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
