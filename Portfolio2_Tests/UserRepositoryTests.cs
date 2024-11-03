using DataAcessLayer.Context;
using DataAcessLayer.Entities.Users;
using DataAcessLayer.Repositories.Implementations;
using DataAcessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Portfolio2_Tests
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly IUserRepository _repository;
        private readonly TestDbContext _context;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ImdbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TestDbContext(options);

            // Seed test data
            _context.Users.Add(new User
            {
                Username = "testuser",
                Email = "testuser@test.com",
                PasswordHash = "password123", // In real implementation, this should be hashed
                CreatedAt = DateTime.UtcNow,
                Role = "User"
            });
            _context.SaveChanges();

            _repository = new UserRepository(_context);
        }

        [Fact]
        public async Task LoginUserAsync_Should_Return_True_For_Valid_Credentials()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";

            // Act
            var result = await _repository.LoginUserAsync(username, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RegisterUserAsync_Should_Create_New_User()
        {
            // Arrange
            string username = "newuser";
            string email = "newuser@test.com";
            string password = "password123";

            // Act
            await _repository.RegisterUserAsync(username, email, password);

            // Assert
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            Assert.NotNull(user);
            Assert.Equal(email, user.Email);
            Assert.Equal(password, user.PasswordHash);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}