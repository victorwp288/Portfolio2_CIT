
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
[Route("api/registration")]
public class RegistrationController : BaseController
{
    private readonly IConfiguration _configuration;
    IUserService _userService;
    IDataService _dataService;
    private readonly LinkGenerator _linkGenerator;

    public RegistrationController(
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
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRegistrationModel model)
    {

        var dto = new UserRegistrationDTO { 
            Email = model.Email,
            Username = model.Username,
            Password = model.Password
        };
        var user = await _userService.RegisterUserAsync(dto);
        return Ok(model);
    }    
}
