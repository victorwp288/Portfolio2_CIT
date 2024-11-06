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

namespace WebServiceLayer.Controllers.Users;

// Attribute indicating this class is an API controller, and setting the base route to "api/movies"
[ApiController]
[Route("api/users")]
public class UserController : BaseController
{
    IBookmarkService _bookmarkService;
    IUserService _userService;

	ISearchService _searchService;
    private readonly LinkGenerator _linkGenerator;

    public UserController(
        IUserService userService,
        IBookmarkService bookmarkService,
		ISearchService searchService, 
        LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _bookmarkService = bookmarkService;
        _userService = userService;
        _linkGenerator = linkGenerator;
		_searchService = searchService;

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

    //update user
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(int userId, UserUpdateDTO model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        model.UserId = userId;
        await _userService.UpdateUserAsync(model);
        return Ok();
    }

    //delete user
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await _userService.DeleteUserAsync(userId);
        return Ok();
    }

    [HttpPost("{userId}/{tconst}")]
    
    //create a bookmark
    public async Task<IActionResult> CreateBookmark(CreateBookmarkModel model)
    {
        var DTO = model.Adapt<BookmarkDTO>();
        await _bookmarkService.CreateBookmarkAsync(DTO);
        return Ok();
    }

    //delete a bookmark
    [HttpDelete("{userId}/{tconst}")]
    public async Task<IActionResult> DeleteBookmark(int userId, string tconst)
    {
        await _bookmarkService.DeleteBookmarkAsync(userId, tconst); 
        return Ok();
    }

    // Delete user search history
    [HttpDelete("{userId}/search-history")]
    public async Task<IActionResult> DeleteSearchHistory(int userId)
    {
        await _searchService.DeleteUserSearchHistoryAsync(userId);
        return Ok();
    }

}
