using Xunit;
using BusinessLayer.Services;
using BusinessLayer.Interfaces;
using BusinessLayer.DTOs;
using Microsoft.EntityFrameworkCore;
using DataAcessLayer;
using DataAcessLayer.Movies;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Portfolio2_Tests
{
    public class TitleServiceTests : IDisposable
    {
        private readonly ITitleService _titleService;
        private readonly TestDbContext _context;

        public TitleServiceTests()
        {
            var options = new DbContextOptionsBuilder<ImdbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            // Seed the database with initial data
            SeedDatabase();

            _titleService = new TitleService(_context);
        }

        private void SeedDatabase()
        {
            var title1 = new TitleBasic
            {
                Tconst = "tt0000001",
                PrimaryTitle = "Test Movie 1",
                OriginalTitle = "Test Movie 1",
                IsAdult = false,
                StartYear = "2020",
                TitleType = "movie",
                TitleRating = new TitleRating
                {
                    Tconst = "tt0000001",
                    AverageRating = 8.5m,
                    NumVotes = 1000
                },
                MovieGenres = new List<MovieGenre>
                {
                    new MovieGenre { Tconst = "tt0000001", Genre = "Drama" },
                    new MovieGenre { Tconst = "tt0000001", Genre = "Action" }
                }
            };

            var title2 = new TitleBasic
            {
                Tconst = "tt0000002",
                PrimaryTitle = "Test Movie 2",
                OriginalTitle = "Test Movie 2",
                IsAdult = false,
                StartYear = "2021",
                TitleType = "movie",
                TitleRating = new TitleRating
                {
                    Tconst = "tt0000002",
                    AverageRating = 9.0m,
                    NumVotes = 2000
                },
                MovieGenres = new List<MovieGenre>
                {
                    new MovieGenre { Tconst = "tt0000002", Genre = "Comedy" }
                }
            };

            _context.TitleBasics.AddRange(title1, title2);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetTitleByIdAsync_Should_Return_TitleDTO_When_Title_Exists()
        {
            // Arrange
            var tconst = "tt0000001";

            // Act
            var result = await _titleService.GetTitleByIdAsync(tconst);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tconst, result.TConst);
            Assert.Equal("Test Movie 1", result.PrimaryTitle);
            Assert.Equal(8.5m, result.AverageRating);
            Assert.Contains("Drama", result.Genres);
            Assert.Contains("Action", result.Genres);
        }

        [Fact]
        public async Task GetTitleByIdAsync_Should_Throw_When_Title_Not_Found()
        {
            // Arrange
            var tconst = "tt9999999";

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _titleService.GetTitleByIdAsync(tconst));
        }

        [Fact]
        public async Task SearchTitlesAsync_Should_Return_Titles_Matching_Query()
        {
            // Arrange
            var query = "Test Movie";

            // Act
            var results = await _titleService.SearchTitlesAsync(query);

            // Assert
            Assert.NotNull(results);
            Assert.Equal(2, results.Count());
        }

        [Fact]
        public async Task GetTopRatedTitlesAsync_Should_Return_Top_Rated_Titles()
        {
            // Arrange
            var count = 1;

            // Act
            var results = await _titleService.GetTopRatedTitlesAsync(count);

            // Assert
            Assert.NotNull(results);
            var topTitle = results.First();
            Assert.Equal("tt0000002", topTitle.TConst);
            Assert.Equal(9.0m, topTitle.AverageRating);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
