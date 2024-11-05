using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataAcessLayer.Context;
using DataAcessLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Portfolio2_Tests
{
    public class UserServiceTests : IDisposable
    {
        private readonly IUserService _userService;
        private readonly TestDbContext _context;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<ImdbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            // Seed the database with initial data
            SeedDatabase();

            _userService = new UserService(_context);
        }

        private void SeedDatabase()
        {
            _context.Users.Add(new User
            {
                UserId = 1,
                Email = "existinguser@example.com",
                Username = "ExistingUser",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow,
                Role = UserRole.user
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task RegisterUserAsync_Should_Throw_When_Email_Exists()
        {
            // Arrange
            var registrationDto = new UserRegistrationDTO
            {
                Email = "existinguser@example.com", // Email already exists
                Username = "AnotherUser",
                Password = "password123"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.RegisterUserAsync(registrationDto));
        }

        [Fact]
        public async Task AuthenticateUserAsync_Should_Throw_When_Credentials_Are_Invalid()
        {
            // Arrange
            var email = "nonexistentuser@example.com";
            var password = "wrongpassword";

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.AuthenticateUserAsync(email, password));
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
