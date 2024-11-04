using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace WebServiceLayer.Controllers.Users
{

    [ApiController]
    [Route("api/user/review")]
    public class ReviewController : BaseController
    {
        IRatingService _ratingService;
        private readonly LinkGenerator _linkGenerator;

        public ReviewController(
            IRatingService ratingService,
            LinkGenerator linkGenerator)
             : base(linkGenerator)
        {
            _linkGenerator = linkGenerator;
            _ratingService = ratingService;
        }
        [HttpGet("{id,tconst}", Name = nameof(GetReview))]
        public async Task<IActionResult> GetReview(int id, string tconst)
        {
            var result = await _ratingService.GetUserRatingAsync(id, tconst);
            return Ok(result);
        }

    }
}
