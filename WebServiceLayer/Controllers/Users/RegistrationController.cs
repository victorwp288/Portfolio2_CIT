
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models.Users;

namespace WebServiceLayer.Controllers.Users;
[ApiController]
[Route("api/registration")]
public class RegistrationController : BaseController
{
    IUserService _userService;
    private readonly LinkGenerator _linkGenerator;

    public RegistrationController(
        IUserService userService,
        LinkGenerator linkGenerator)
         : base(linkGenerator)
    {
        _linkGenerator = linkGenerator;
        _userService = userService;
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
