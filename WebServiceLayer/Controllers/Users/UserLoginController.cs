

using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAcessLayer;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using DataAcessLayer.Entities.Users;
using System.Security.Claims;

namespace WebServiceLayer.Controllers.Users;
[ApiController]
[Route("api/login")]
public class UserLoginController : BaseController
{
    private readonly IConfiguration _configuration;
    IUserService _userService;
    IDataService _dataService;
    private readonly LinkGenerator _linkGenerator;

    public UserLoginController(
        IUserService userService,
        IDataService dataService,
        LinkGenerator linkGenerator, IConfiguration configuration)
         : base(linkGenerator)
    {
        _linkGenerator = linkGenerator;
        _userService = userService;
        _dataService = dataService;
        _configuration = configuration;
    }
    [HttpPost] // Use HttpPost for login (sending data)
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
}