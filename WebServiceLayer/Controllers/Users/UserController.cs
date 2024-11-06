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

    //register a new user
    [HttpPost("register")]
    public async Task<IActionResult> CreateUser(UserRegistrationDTO model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userService.RegisterUserAsync(model);
        return Ok(model);
    }

    //update user
    [HttpPut("{userId}/update")]
    public async Task<IActionResult> UpdateUser(int userId, UserUpdateDTO model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        model.UserId = userId;
        await _userService.UpdateUserAsync(model);
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

}
