using DataAcessLayer.Entities.Movies;
using DataAcessLayer.Entities.Users;
using DataAcessLayer.Repositories.Implementations;

namespace Portfolio2_Tests;

public class DataServiceTests : BaseRepositoryTests
{
    private readonly IDataService _service;

    public DataServiceTests() : base()
    {
        _service = new DataService(_context);
        SeedTestData();
    }

    private void SeedTestData()
    {
        var user = new User
        {
            UserId = 1,
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hashedpassword",
            CreatedAt = DateTime.UtcNow,
            Role = "User"
        };

        var titleBasic = new TitleBasic
        {
            Tconst = "tt0000001",
            TitleType = "movie",
            PrimaryTitle = "Test Movie",
            OriginalTitle = "Test Movie",
            IsAdult = false,
            StartYear = "2023"
        };

        _context.Users.Add(user);
        _context.TitleBasics.Add(titleBasic);
        _context.SaveChanges();
    }

    [Fact]
    public void GetUsers_Should_Return_All_Users()
    {
        // Act
        var users = _service.GetUsers();

        // Assert
        Assert.NotNull(users);
        Assert.Single(users);
        Assert.Equal("testuser", users.First().Username);
    }

    [Fact]
    public void GetTitleBasic_Should_Return_Correct_Title()
    {
        // Act
        var title = _service.GetTitleBasic("tt0000001");

        // Assert
        Assert.NotNull(title);
        Assert.Equal("Test Movie", title.PrimaryTitle);
    }
}
