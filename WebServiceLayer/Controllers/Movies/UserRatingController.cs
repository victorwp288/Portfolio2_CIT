using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models.Movies;


namespace WebServiceLayer.Controllers.Movies;

[ApiController]
[Route("api/ratings")]
public class UserRatingController : BaseController
{
    IRatingService _ratingService;
    private readonly LinkGenerator _linkGenerator;

    public UserRatingController(
        IRatingService ratingService,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _ratingService = ratingService;
        _linkGenerator = linkGenerator;
    }

    /*[HttpPost]
    public async Task<IActionResult> SubmitUserRatingAsync(UserRating ratingModel)
    {
        var ratingDto = ratingModel.Adapt<UserRatingDTO>();
        await _ratingService.SubmitUserRatingAsync(ratingDto);

        return CreatedAtAction(nameof(GetUserRatingAsync), new { userId = ratingDto.UserId, tconst = ratingDto.Tconst }, ratingModel);
    }*/

    [HttpGet("{userId}/{tconst}", Name = nameof(GetUserRatingAsync))]
    public async Task<IActionResult> GetUserRatingAsync(int userId, string tconst)
    {
        var rating = await _ratingService.GetUserRatingAsync(userId, tconst);

        if (rating == null)
        {
            return NotFound();
        }

        var model = CreateRating(rating);

        return Ok(model);
    }

    private UserRating CreateRating(UserRatingDTO rating)
    {
        var model = rating.Adapt<UserRating>();

        model.Url = GetUrl(nameof(GetUserRatingAsync), new { userId = rating.UserId, tconst = rating.TConst });

        return model;
    }

}
