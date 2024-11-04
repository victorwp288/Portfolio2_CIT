using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataAcessLayer;
using DataAcessLayer.Entities.Movies;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.Models.Movies;

namespace WebServiceLayer.Controllers.Movies;

    // Attribute indicating this class is an API controller, and setting the base route to "api/movies"
    [ApiController]
    [Route("api/movies")]
    public class SearchResultController : BaseController
    {
        // accessing data from the data service layer, and generating links within the controller
        IDataService _dataService;
        ISearchService _searchService;
        private readonly LinkGenerator _linkGenerator;

        // Constructor that injects IDataService and LinkGenerator via dependency injection
        public SearchResultController(
        IDataService dataService,
        ISearchService searchService,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
        {
            _dataService = dataService;
            _searchService = searchService;
            _linkGenerator = linkGenerator;
        }

           // GET method to retrieve a list of movies based on search result
        [HttpGet("{search}", Name = nameof(SearchAsync))]
        public IActionResult SearchAsync(string search)
        {
            // Retrieve a list of movies for the specified search, converting each to SearchResultModel
            var searchresults = _searchService
                .Select(SearchResultModel);

            return Ok(searchresults);
        }


    private SearchResultDTO SearchResultModel(SearchResultDTO searchresult)
        {
            // If the title is null, return null (avoiding null reference exceptions)
            if (searchresult == null)
            {
                return null;
            }
            // Map TitleBasic entity properties to TitleBasicModel properties
            var model = searchresult.Adapt<SearchResultDTO>();

            // Generate URL for accessing details of the current movie and add to the model
            model.Url = GetUrl(nameof(SearchAsync), new { searchresult.Name });

            return model;
        }

    }
