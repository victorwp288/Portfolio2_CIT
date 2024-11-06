using Microsoft.AspNetCore.Mvc;
using Mapster;
using BusinessLayer.Interfaces;
using BusinessLayer.DTOs;
using WebServiceLayer.Models.Users;
using WebServiceLayer.Models.Movies;
using DataAcessLayer;
namespace WebServiceLayer.Controllers.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

// Attribute indicating this class is an API controller, and setting the base route to "api/movies"
[ApiController]
[Route("api/users")]
public class UserController : BaseController
{
    private readonly IConfiguration _configuration;
    IUserService _userService;
    IDataService _dataService;
    private readonly LinkGenerator _linkGenerator;

    public UserController(
        IUserService userService,
        IConfiguration configuration,
        IDataService dataService,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _userService = userService;
        _linkGenerator = linkGenerator;
        _configuration = configuration;
    }

    [HttpGet("{userId}", Name = nameof(GetUserByIdAsync))]
    public async Task<IActionResult> GetUserByIdAsync(int userId)
    {
        var category = await _userService.GetUserByIdAsync(userId);

        if (category == null)
        {
            return NotFound();
        }
        var model = CreateUserModel(category);

        return Ok(model);
    }

    //map the user entity to the user model
    private UserModel CreateUserModel([FromBody] UserDTO user)
    {
        // Map TitleBasic entity properties to MovieModel properties
        var model = user.Adapt<UserModel>();

        // Generate URL for accessing details of the current movie and add to the model
        model.Url = GetUrl(nameof(GetUserByIdAsync), new { user.UserId });

        return model;
    }

    //creating a new user
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserRegistrationDTO model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userService.RegisterUserAsync(model);
        return Ok(model);
    }

}
