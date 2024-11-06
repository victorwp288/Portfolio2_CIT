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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DataAcessLayer.Context;
using System.ComponentModel.DataAnnotations;


namespace WebServiceLayer.Controllers.Users;

// Attribute indicating this class is an API controller, and setting the base route to "api/users"
[ApiController]
[Route("api/users")]
public class UserController : BaseController
{
    private readonly IConfiguration _configuration;
    IDataService _dataService;
    IBookmarkService _bookmarkService;
    IUserService _userService;
	ISearchService _searchService;
    private readonly ImdbContext _context;
    private readonly LinkGenerator _linkGenerator;

    public UserController(
        IConfiguration configuration,
        IDataService dataService,
        IUserService userService,
        IBookmarkService bookmarkService,
		ISearchService searchService, 
        LinkGenerator linkGenerator,
        ImdbContext context)
        : base(linkGenerator)
    {
        _configuration = configuration;
        _dataService = dataService;
        _bookmarkService = bookmarkService;
        _userService = userService;
        _linkGenerator = linkGenerator;
		_searchService = searchService;
        _context = context;
    }


    // Login method
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginModel model)
    {
        try
        {
            // Log the incoming credentials (for debugging only - remove in production!)
            Console.WriteLine($"Login attempt - Username: {model.UserName}, Password: {model.Password}");

            // Get user details first and log what we find
            var user = _context.Users.FirstOrDefault(u => u.Username == model.UserName);
            
            if (user == null)
            {
                Console.WriteLine("No user found with that username");
                return Unauthorized(new { message = "Invalid username or password" });
            }

            Console.WriteLine($"Found user - Username: {user.Username}, StoredPassword: {user.PasswordHash}");

            // Check password match
            if (user.PasswordHash != model.Password)
            {
                Console.WriteLine("Password mismatch");
                return Unauthorized(new { message = "Invalid username or password" });
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            // Return the token and user info
            return Ok(new
            {
                token = token,
                userId = user.UserId,
                username = user.Username,
                role = user.Role.ToString()
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return StatusCode(500, new { message = "An error occurred during login" });
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

    //helper method to generate JWT token
    private string GenerateJwtToken(DataAcessLayer.Entities.Users.User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}