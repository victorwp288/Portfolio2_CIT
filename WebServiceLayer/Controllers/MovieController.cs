using DataAcessLayer;
using DataAcessLayer.Entities.Movies;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models;


namespace WebServiceLayer.Controllers
{
    // Attribute indicating this class is an API controller, and setting the base route to "api/movies"
    [ApiController]
    [Route("api/movies")]
    public class MovieController : BaseController
    {
        // accessing data from the data service layer, and generating links within the controller
        IDataService _dataService;
        private readonly LinkGenerator _linkGenerator;

        // Constructor that injects IDataService and LinkGenerator via dependency injection
        public MovieController(
        IDataService dataService,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
        {
            _dataService = dataService;
            _linkGenerator = linkGenerator;
        }

        // GET method to retrieve a specific movie by its unique identifier
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

        // GET method to retrieve a list of movies, with optional pagination parameters
        [HttpGet(Name = nameof(GetMovies))]
        public IActionResult GetMovies(int page = 0, int pageSize = 5)
        {
            // Retrieve a list of movies for the specified page and size, converting each to TitleBasicModel
            var categories = _dataService
                .GetTitleBasics(page, pageSize)
                .Select(CreateTitleBasicModel);

            var numberOfItems = _dataService.GetNumberOfTitleBasics();

            // Create a paginated response object with links and metadata for paging
            object result = CreatePaging(
                nameof(GetMovies),
                page,
                pageSize,
                numberOfItems,
                categories);
            return Ok(result);
        }

        // Helper method to create a TitleBasicModel from a TitleBasic entity
        private TitleBasicModel? CreateTitleBasicModel(TitleBasic? title)
        {
            // If the title is null, return null (avoiding null reference exceptions)
            if (title == null)
            {
                return null;
            }
            // Map TitleBasic entity properties to TitleBasicModel properties
            var model = title.Adapt<TitleBasicModel>();

            // Generate URL for accessing details of the current movie and add to the model
            model.Url = GetUrl(nameof(GetMovies), new { title.Tconst });

            return model;
        }
    }
}