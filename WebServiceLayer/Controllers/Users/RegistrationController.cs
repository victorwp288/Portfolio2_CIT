
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAcessLayer;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models.Users;

namespace WebServiceLayer.Controllers.Users;
[ApiController]
[Route("api/registration")]
public class RegistrationController : BaseController
{
    IUserService _userService;
    IDataService _dataService;
    private readonly LinkGenerator _linkGenerator;

    public RegistrationController(
        IUserService userService,
        IDataService dataService,
        LinkGenerator linkGenerator)
         : base(linkGenerator)
    {
        _linkGenerator = linkGenerator;
        _userService = userService;
        _dataService = dataService;
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
    [HttpGet]
    public int UserLogin(UserLoginModel model)
    {

        return _dataService.FunctionRegisterUser(model.UserName, model.Email, model.Password);
    }
}
