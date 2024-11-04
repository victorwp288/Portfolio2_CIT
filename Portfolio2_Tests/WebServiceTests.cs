﻿using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
           // LinkGenerator generator = new ();
           // _controller = new RegistrationController(_userService, )

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

    }
}
