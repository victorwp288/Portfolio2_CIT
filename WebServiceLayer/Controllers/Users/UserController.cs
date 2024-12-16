using Microsoft.AspNetCore.Mvc;
using Mapster;
using BusinessLayer.Interfaces;
using BusinessLayer.DTOs;
using WebServiceLayer.Models.Users;
using DataAcessLayer;
using BuisnessLayer.Interfaces;
using DataAcessLayer.Repositories.Implementations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace WebServiceLayer.Controllers.Users;

// Attribute indicating this class is an API controller, and setting the base route to "api/users"
[ApiController]
[Route("api/users")]
public class UserController : BaseController
{
    private readonly IConfiguration _configuration;
    IHasherService _hasherService;
    IDataService _dataService;
    IBookmarkService _bookmarkService;
    IUserService _userService;
    ISearchService _searchService;
    private readonly LinkGenerator _linkGenerator;
    private DataService _service;
    ITitleService _titleService;

    public UserController(
        IConfiguration configuration,
        IHasherService hasherService,
        ITitleService titleService,
        IDataService dataService,
        IUserService userService,
        IBookmarkService bookmarkService,
        ISearchService searchService,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _configuration = configuration;
        _hasherService = hasherService;
        _dataService = dataService;
        _bookmarkService = bookmarkService;
        _userService = userService;
        _linkGenerator = linkGenerator;
        _searchService = searchService;
    }


    // Login method
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginModel model)
    {
        try
        {
            // Log the incoming credentials (for debugging only - remove in production!)
            Console.WriteLine($"Login attempt - Username: {model.UserName}, Password: {model.Password}");

            var user = await _dataService.GetUserByUserNameAsync(model.UserName);
            if (user == null)
            {
                Console.WriteLine("No user found with that username");
                return Unauthorized(new { message = "Invalid Username" });
            }

            Console.WriteLine($"Found user - Username: {user.Username}, StoredPassword: {user.PasswordHash}");
            
            var logIn = await _userService.LoginUserAsync(model.UserName, model.Password);

            if (logIn)
            {

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
            else { throw new Exception("Invalid Password"); }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return StatusCode(500, new { message = ex.Message });
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

        try
        {
            
            var userByUsername = await _dataService.GetUserByUserNameAsync(model.Username);
            if (userByUsername != null)
            {
                Console.WriteLine("An user found with this username");
                return Unauthorized(new { message = "An user exists with this UserName." });
            }
            else
            {
                var userByEmail = await _dataService.GetUserByEmailAsync(model.Email);
                if (userByEmail != null)
                {
                    Console.WriteLine("An user found with this email");
                    return Unauthorized(new { message = "An user exists with this Email." });
                }
                else
                {
                    var dto = new UserRegistrationDTO
                    {
                        Email = model.Email,
                        Username = model.Username,
                        Password = model.Password
                    };
                    var user = await _userService.RegisterUserAsync(dto);
                    if(user != null)
                        return Ok(user);
                    else
                        return Unauthorized(new { message = "Something went wrong, Please try later." });
                }


            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    //update user
    [HttpPut("{userId}/update")]

    public async Task<IActionResult> UpdateUser(int userId, UserUpdateModel model)
    {

        if (model == null || userId <= 0)
        {
            return BadRequest("Invalid input model or user ID");//400
        }
        var dto = new UserUpdateDTO
        {
            UserId = userId,
            Email = model.Email,
            Username = model.Username,
            Password = model.Password
        };

        try
        {
            bool result = await _userService.UpdateUserAsync(dto);
            if (result)
            {
                return Ok("User updated successfully"); //200
            }
            else
            {
                return NotFound($"User with ID {userId} not found."); //404
            }
        }
        catch (Exception ex)
        {
            //Log the exception
            Console.WriteLine($"Error updating user: {ex.Message}");
            return StatusCode(500, "An error occurred while updating user");
        }
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
        
        var result = _dataService.FunctionAddBookmark(userId, tconst, "n/a");

        if (!result)//|| validTconst == null)
        {
            return NotFound();
        }
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
        var bookmarks = _dataService.GetUserBookmerksByUserId(userId);
        Console.WriteLine(bookmarks.First().Tconst);
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