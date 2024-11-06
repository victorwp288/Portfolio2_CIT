using DataAcessLayer;
using DataAcessLayer.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebServiceLayer.Controllers.Users;

// Attribute indicating this class is an API controller, and setting the base route to "api/users"
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IDataService _dataService;
    private readonly ImdbContext _context;

    public UserController(
        IConfiguration configuration,
        IDataService dataService,
        ImdbContext context)
    {
        _configuration = configuration;
        _dataService = dataService;
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
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

public class LoginModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}
