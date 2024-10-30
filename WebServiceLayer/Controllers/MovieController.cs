using Microsoft.AspNetCore.Mvc;
using DataAcessLayer;
using WebServiceLayer.Controllers;

namespace WebServiceLayer.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MovieController : BaseController
    {
        IDataService _dataService;
        private readonly LinkGenerator _linkGenerator;

        public MovieController(
        IDataService dataService,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
        {
            _dataService = dataService;
            _linkGenerator = linkGenerator;
        }

       /* [HttpGet(Name = nameof(GetMovies))]
        public IActionResult GetMovies(int page = 0, int pageSize = 2)
        {
            var categories = _dataService
                .GetTitleBasic(page, pageSize)
                .Select(CreateCategoryModel);
            var numberOfItems = _dataService.NumberOfCategories();
            object result = CreatePaging(
                nameof(GetCategories),
                page,
                pageSize,
                numberOfItems,
                categories);
            return Ok(result);
        }*/
    }
}
