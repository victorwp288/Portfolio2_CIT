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
using System.Linq;

namespace Portfolio2_Tests
{
    public class SearchServiceTests : IDisposable
    {
        private readonly ISearchService _searchService;
        private readonly TestDbContext _context;

        public SearchServiceTests()
        {
            var options = new DbContextOptionsBuilder<ImdbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            // Seed the database with initial data
            SeedDatabase();

            _searchService = new SearchService(_context);
        }

        private void SeedDatabase()
        {
            // Add titles
            _context.TitleBasics.AddRange(
                new TitleBasic
                {
                    Tconst = "tt0000001",
                    TitleType = "movie",
                    PrimaryTitle = "Searchable Movie",
                    OriginalTitle = "Searchable Movie",
                    IsAdult = false,
                    StartYear = "2020",
                    RunTimeMinutes = 120
                },
                new TitleBasic
                {
                    Tconst = "tt0000002",
                    TitleType = "movie",
                    PrimaryTitle = "Another Film",
                    OriginalTitle = "Another Film",
                    IsAdult = false,
                    StartYear = "2021",
                    RunTimeMinutes = 90
                }
            );

            // Add persons
            _context.NameBasics.AddRange(
                new NameBasic
                {
                    Nconst = "nm0000001",
                    PrimaryName = "Searchable Actor",
                    BirthYear = "1980",
                    // Include other required properties
                },
                new NameBasic
                {
                    Nconst = "nm0000002",
                    PrimaryName = "Another Actor",
                    BirthYear = "1985",
                    // Include other required properties
                }
            );

            _context.SaveChanges();

            // Verify counts
            var titleCount = _context.TitleBasics.Count();
            var nameCount = _context.NameBasics.Count();

            Console.WriteLine($"Titles in DB: {titleCount}");
            Console.WriteLine($"Names in DB: {nameCount}");
        }


        [Fact]
        public async Task SearchAsync_Should_Return_Titles_And_Persons_Matching_Query()
        {
            // Arrange
            var query = "Searchable";

            // Act
            var results = await _searchService.SearchAsync(query);

            // Assert
            Assert.NotNull(results);
            Assert.Equal(2, results.Count()); // One title and one person

            var titleResult = results.FirstOrDefault(r => r.Type == "Title");
            var personResult = results.FirstOrDefault(r => r.Type == "Person");

            Assert.NotNull(titleResult);
            Assert.Equal("Searchable Movie", titleResult.Name);

            Assert.NotNull(personResult);
            Assert.Equal("Searchable Actor", personResult.Name);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
