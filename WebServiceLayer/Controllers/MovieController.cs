using Microsoft.AspNetCore.Mvc;
using DataAcessLayer;
using DataAcessLayer.Movies;
using WebServiceLayer.Models;
using Mapster;


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

        [HttpGet("{id}", Name = nameof(GetTitleBasic))]
        public IActionResult GetTitleBasic(string id)
        {
            var category = _dataService.GetTitleBasic(id);

            if (category == null)
            {
                return NotFound();
            }
            var model = CreateTitleBasicModel(category);

            return Ok(model);
        }

        [HttpGet(Name = nameof(GetMovies))]
        public IActionResult GetMovies(int page = 0, int pageSize = 5)
        {
            var categories = _dataService
                .GetTitleBasics(page, pageSize)
                .Select(CreateTitleBasicModel);
            var numberOfItems = _dataService.GetNumberOfTitleBasics();
            object result = CreatePaging(
                nameof(GetMovies),
                page,
                pageSize,
                numberOfItems,
                categories);
            return Ok(result);
        }

        private TitleBasicModel? CreateTitleBasicModel(TitleBasic? title)
        {
            if (title == null)
            {
                return null;
            }

            var model = title.Adapt<TitleBasicModel>();
            model.Url = GetUrl(nameof(GetMovies), new { title.Tconst });

            return model;
        }
    }
}
