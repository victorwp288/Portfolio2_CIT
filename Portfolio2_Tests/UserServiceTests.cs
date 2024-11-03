using Xunit;
using BusinessLayer.Services;
using BusinessLayer.Interfaces;
using BusinessLayer.DTOs;
using Microsoft.EntityFrameworkCore;
using DataAcessLayer.Entities.Users;
using System.Threading.Tasks;
using System;
using DataAcessLayer.Context;

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
            // Add a user to simulate existing data
            _context.Users.Add(new User
            {
                UserId = 1,
                Email = "existinguser@example.com",
                Username = "ExistingUser",
                PasswordHash = "password123", // For testing purposes
                CreatedAt = DateTime.UtcNow,
                Role = "User"
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task RegisterUserAsync_Should_Register_New_User()
        {
            // Arrange
            var registrationDto = new UserRegistrationDTO
            {
                Email = "newuser@example.com",
                Username = "NewUser",
                Password = "password123"
            };

            // Act
            var result = await _userService.RegisterUserAsync(registrationDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(registrationDto.Email, result.Email);
            Assert.Equal(registrationDto.Username, result.Username);
            Assert.Equal("User", result.Role);

            // Verify user is in the database
            var userInDb = await _context.Users.SingleOrDefaultAsync(u => u.Email == registrationDto.Email);
            Assert.NotNull(userInDb);
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
        public async Task AuthenticateUserAsync_Should_Return_UserDTO_When_Credentials_Are_Correct()
        {
            // Arrange
            var email = "existinguser@example.com";
            var password = "password123";

            // Act
            var result = await _userService.AuthenticateUserAsync(email, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
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
