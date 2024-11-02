using Microsoft.AspNetCore.Mvc;
using DataAcessLayer;
using DataAcessLayer.Movies;
using WebServiceLayer.Models;
using Mapster;
using BusinessLayer.Interfaces;
using BusinessLayer.DTOs;
using BusinessLayer.Services;

namespace WebServiceLayer.Controllers.Users;

    // Attribute indicating this class is an API controller, and setting the base route to "api/movies"
    [ApiController]
    [Route("api/users")]
    public class UserController : BaseController
    {
        
        IUserService _userService;
        private readonly LinkGenerator _linkGenerator;

        public UserController(
            IUserService userService,
            LinkGenerator linkGenerator)
            : base(linkGenerator)
        {
            _userService = userService;
            _linkGenerator = linkGenerator;
        }



    
    }
