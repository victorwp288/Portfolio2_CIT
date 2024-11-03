using Microsoft.AspNetCore.Mvc;
using Mapster;
using BusinessLayer.Interfaces;
using BusinessLayer.DTOs;
using WebServiceLayer.Models.Users;
using WebServiceLayer.Models.Movies;
using DataAcessLayer;   

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

        [HttpGet("{userId}", Name = nameof(GetUserByIdAsync))]
        public async Task<IActionResult> GetUserByIdAsync(int userId)
        {
            var category = await _userService.GetUserByIdAsync(userId);

            if (category == null)
            {
                return NotFound();
            }
            var model = CreateUser(category);

            return Ok(model);
        }


    private User CreateUser(UserDTO user)
        {
            // Map TitleBasic entity properties to MovieModel properties
            var model = user.Adapt<User>();

            // Generate URL for accessing details of the current movie and add to the model
            model.Url = GetUrl(nameof(GetUserByIdAsync), new { user.UserId });

            return model;
        }


    }
