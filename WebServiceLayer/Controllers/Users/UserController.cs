using Microsoft.AspNetCore.Mvc;
using Mapster;
using BusinessLayer.Interfaces;
using BusinessLayer.DTOs;
using WebServiceLayer.Models.Users;
using WebServiceLayer.Models.Movies;
using DataAcessLayer;
using DataAccessLayer.Repositories.Implementations;
using DataAcessLayer.Repositories.Interfaces;
using BuisnessLayer.Interfaces;
using BuisnessLayer.DTOs;
using DataAcessLayer.Repositories.Implementations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace WebServiceLayer.Controllers.Users;

// Attribute indicating this class is an API controller, and setting the base route to "api/movies"
[ApiController]
[Route("api/users")]
public class UserController : BaseController
{
    private readonly IConfiguration _configuration;
    IDataService _dataService;
    IBookmarkService _bookmarkService;
    IUserService _userService;
	ISearchService _searchService;
    private readonly LinkGenerator _linkGenerator;

    public UserController(
        IConfiguration configuration,
        IDataService dataService,
        IUserService userService,
        IBookmarkService bookmarkService,
		ISearchService searchService, 
        LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _configuration = configuration;
        _dataService = dataService;
        _bookmarkService = bookmarkService;
        _userService = userService;
        _linkGenerator = linkGenerator;
		_searchService = searchService;

    }

    //login user
    [HttpPost("login")] // Use HttpPost for login (sending data)
    public IActionResult UserLogin(UserLoginModel model)
    {
        // Validate user input
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Return validation errors
        }

        // Authenticate user
        var userId = _dataService.FunctionLoginUser(model.UserName, model.Password);

        // Handle authentication result
        if (userId) // Assuming userId is non-zero on successful login
        {
            // Create JWT token
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserId", model.UserName.ToString()) // Use "UserId" for consistency
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); // Correctly initialize signIn

            // 2. Generate the JWT token
            var token = new JwtSecurityToken(
                //_configuration["Jwt:Issuer"],
                //_configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn // Use signIn here
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Return the token and userId
            return Ok(new { Token = tokenString });
        }
        else
        {
            // Handle authentication failure
            return Unauthorized(); // Return Unauthorized status code
        }
    }

    //get user by id
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

    //register a new user
    [HttpPost("register")]
    public async Task<IActionResult> CreateUser(CreateUserRegistrationModel model)
    {

        var dto = new UserRegistrationDTO
        {
            Email = model.Email,
            Username = model.Username,
            Password = model.Password
        };
        var user = await _userService.RegisterUserAsync(dto);
        return Ok(model);
    }

    //update user
    [HttpPut("{userId}/update")]
    public async Task<IActionResult> UpdateUser(int userId, UserUpdateModel model)
    {
        var dto = new UserUpdateDTO
        {
            UserId= userId,
            Email = model.Email,
            Username = model.Username,
            Password = model.Password
        };
        await _userService.UpdateUserAsync(dto);
        return Ok();
    }

    //delete user
    [HttpDelete("{userId}/delete")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await _userService.DeleteUserAsync(userId);
        return Ok();
    }

    [HttpPost("{userId}/{tconst}/bookmark")]
    public async Task<IActionResult> CreateBookmark(int userId, string tconst, CreateBookmarkModel model)
    {
        var DTO = model.Adapt<BookmarkDTO>();
        await _bookmarkService.CreateBookmarkAsync(userId, tconst, DTO);
        return Ok();
    }

    //delete a bookmark
    [HttpDelete("{userId}/{tconst}/bookmark")]
    public async Task<IActionResult> DeleteBookmark(int userId, string tconst)
    {
        await _bookmarkService.DeleteBookmarkAsync(userId, tconst); 
        return Ok();
    }

    [HttpGet("{userId}/bookmarks")]
    public async Task<IActionResult> GetUserBookmarks(int userId)
    {
        var bookmarks = await _bookmarkService.GetUserBookmarksAsync(userId);
        var model = bookmarks.Adapt<IEnumerable<CreateBookmarkModel>>();
        return Ok(model);
    }

    // Delete user search history
    [HttpDelete("{userId}/search-history")]
    public async Task<IActionResult> DeleteSearchHistory(int userId)
    {
        await _searchService.DeleteUserSearchHistoryAsync(userId);
        return Ok();
    }

    //helper method to map the user entity to the user model
    private UserModel CreateUserModel([FromBody] UserDTO user)
    {
        // Map TitleBasic entity properties to MovieModel properties
        var model = user.Adapt<UserModel>();

        // Generate URL for accessing details of the current movie and add to the model
        model.Url = GetUrl(nameof(GetUserByIdAsync), new { user.UserId });

        return model;
    }
}
