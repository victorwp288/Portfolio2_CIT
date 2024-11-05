using BusinessLayer.DTOs;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models.Movies;
using BusinessLayer.Interfaces;


namespace WebServiceLayer.Controllers.Movies;

[ApiController]
[Route("api/ratings")]
public class UserRatingController : BaseController
{
    private readonly IRatingService _ratingService;
    private readonly LinkGenerator _linkGenerator;

    public UserRatingController(
        IRatingService ratingService,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _ratingService = ratingService;
        _linkGenerator = linkGenerator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRating(CreateRatingModel model)
    {
        var ratingDto = new UserRatingDTO
        {
            UserId = model.UserId,
            TConst = model.TConst,
            Rating = model.Rating,
            Review = model.Review,
            ReviewDate = DateTime.UtcNow
        };

        await _ratingService.SubmitUserRatingAsync(ratingDto);
        
        return Ok(CreateRatingModel(ratingDto));
    }

    [HttpGet("{userId}/{tconst}", Name = nameof(GetUserRatingAsync))]
    public async Task<IActionResult> GetUserRatingAsync(int userId, string tconst)
    {
        var rating = await _ratingService.GetUserRatingAsync(userId, tconst);

        if (rating == null)
        {
            return NotFound();
        }

        var model = CreateRatingModel(rating);
        return Ok(model);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserRatingsAsync(int userId)
    {
        var ratings = await _ratingService.GetUserRatingsAsync(userId);

        if (!ratings.Any())
        {
            return NotFound();
        }

        var models = ratings.Select(CreateRatingModel);
        return Ok(models);
    }

    private UserRating CreateRatingModel(UserRatingDTO rating)
    {
        var model = rating.Adapt<UserRating>();
        model.Url = GetUrl(nameof(GetUserRatingAsync),
            new { userId = rating.UserId, tconst = rating.TConst });
        return model;
    }

    [HttpPut("{userId}/{tconst}")]
    public async Task<IActionResult> UpdateUserRatingAsync(int userId, string tconst, UpdateRatingModel model)
    {
        var rating = await _ratingService.GetUserRatingAsync(userId, tconst);

        rating.Rating = model.Rating;
        rating.Review = model.Review;

        return Ok(rating);
    }
}
