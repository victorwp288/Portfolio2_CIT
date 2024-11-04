using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAcessLayer.Context;
using Microsoft.EntityFrameworkCore;
using WebServiceLayer.Controllers.Users;

namespace Portfolio2_Tests
{
    public class WebServiceTests : IDisposable
    {
        private readonly TestDbContext _context;
        private readonly IUserService _userService;
        private RegistrationController _controller;

        public WebServiceTests()
        {
            var options = new DbContextOptionsBuilder<ImdbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            _context = new TestDbContext(options);
        }

        public async Task RegisterUserAsync_Should_Register_New_User()
        {
            // Arrange
            var registrationDto = new UserRegistrationDTO
            {
                Email = "newuser@example.com",
                Username = "NewUser",
                Password = "password123"
            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
