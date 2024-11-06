using BusinessLayer.DTOs;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models.Movies;
using BusinessLayer.Interfaces;


namespace WebServiceLayer.Controllers.Users;

[ApiController]
[Route("api/users")]
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

    //create user rating
    [HttpPost("{userId}/{tconst}/rating/create")]
    public async Task<IActionResult> CreateRating(int userId, string tconst, CreateRatingModel model)
    {
        var ratingDto = new UserRatingDTO
        {
            UserId = userId,
            TConst = tconst,
            Rating = model.Rating,
            Review = model.Review,
            ReviewDate = DateTime.UtcNow
        };

        await _ratingService.SubmitUserRatingAsync(ratingDto);

        return Ok(CreateRatingModel(ratingDto));
    }

    //get user rating for tconst
    [HttpGet("{userId}/{tconst}/rating", Name = nameof(GetUserRatingAsync))]
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

    //get all user ratings
    [HttpGet("{userId}/rating")]
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

    //update user rating
    [HttpPut("{userId}/{tconst}/rating/update")]
    public async Task<IActionResult> UpdateUserRatingAsync(int userId, string tconst, UpdateRatingModel model)
    {
        var rating = await _ratingService.GetUserRatingAsync(userId, tconst);

        rating.Rating = model.Rating;
        rating.Review = model.Review;

        return Ok(rating);
    }

    //helper method to create rating model from rating DTO
    private UserRating CreateRatingModel(UserRatingDTO rating)
    {
        var model = rating.Adapt<UserRating>();
        model.Url = GetUrl(nameof(GetUserRatingAsync),
            new { userId = rating.UserId, tconst = rating.TConst });
        return model;
    }
}
