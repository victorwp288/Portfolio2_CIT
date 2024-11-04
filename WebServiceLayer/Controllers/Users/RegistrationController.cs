
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> CreateUser(UserRegistrationDTO model)
    {
        var user = await _userService.RegisterUserAsync(model);
        return Ok(model);
    }
}
