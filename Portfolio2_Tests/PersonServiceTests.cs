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
    public class PersonServiceTests : IDisposable
    {
        private readonly IPersonService _personService;
        private readonly TestDbContext _context;

        public PersonServiceTests()
        {
            var options = new DbContextOptionsBuilder<ImdbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            // Seed the database with initial data
            SeedDatabase();

            _personService = new PersonService(_context);
        }

        private void SeedDatabase()
        {
            // Add the title first
            var title = new TitleBasic
            {
                Tconst = "tt0000001",
                TitleType = "movie",           // Ensure all required properties are set
                PrimaryTitle = "Known Movie",
                OriginalTitle = "Known Movie",
                IsAdult = false,
                StartYear = "2000",
            };

            _context.TitleBasics.Add(title);
            _context.SaveChanges();

            // Now add the person and link to the title
            var person = new NameBasic
            {
                Nconst = "nm0000001",
                PrimaryName = "Test Actor",
                BirthYear = "1980",
                PersonProfessions = new List<PersonProfession>
    {
        new PersonProfession { Nconst = "nm0000001", Profession = "Actor" }
    },
                PersonKnownTitles = new List<PersonKnownTitle>
    {
        new PersonKnownTitle
        {
            Nconst = "nm0000001",
            Tconst = "tt0000001",
            TitleBasic = title        // Set the navigation property
        }
    }
            };

            _context.NameBasics.Add(person);
            _context.SaveChanges();

        }

        [Fact]
        public async Task GetPersonByIdAsync_Should_Return_PersonDTO_When_Person_Exists()
        {
            // Arrange
            var nconst = "nm0000001";

            // Act
            var result = await _personService.GetPersonByIdAsync(nconst);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nconst, result.NConst);
            Assert.Equal("Test Actor", result.PrimaryName);
            Assert.Contains("Actor", result.Professions);
            Assert.Single(result.KnownForTitles);
            Assert.Equal("tt0000001", result.KnownForTitles.First().TConst);
        }

        [Fact]
        public async Task GetPersonByIdAsync_Should_Throw_When_Person_Not_Found()
        {
            // Arrange
            var nconst = "nm9999999";

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _personService.GetPersonByIdAsync(nconst));
        }

        [Fact]
        public async Task SearchPersonsAsync_Should_Return_Persons_Matching_Query()
        {
            // Arrange
            var query = "Test Actor";

            // Act
            var results = await _personService.SearchPersonsAsync(query);

            // Assert
            Assert.NotNull(results);
            Assert.Single(results);
            var person = results.First();
            Assert.Equal("nm0000001", person.NConst);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
