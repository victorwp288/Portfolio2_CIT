using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAcessLayer;
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
    [HttpGet("search/{query}", Name = nameof(SearchAsync))]
    public async Task<IActionResult> SearchAsync(string query)
    {
        var searchResults = await _searchService.SearchAsync(query);
        return Ok(searchResults?.Select(x => SearchResultModel(x)));
    }

    [HttpGet("search/titles/{query}", Name = nameof(SearchTitlesAsync))]
    public async Task<IActionResult> SearchTitlesAsync(string query)
    {
        var searchResults = await _searchService.SearchTitleAsync(query);
        return Ok(searchResults.Select(SearchResultModel));
    }

    [HttpGet("search/titles/database/{query}", Name = nameof(SearchTitlesByDatabaseAsync))]
    public async Task<IActionResult> SearchTitlesByDatabaseAsync(string query)
    {
        var searchResults = await _searchService.SearchTitleByDatabaseAsync(query);
        return Ok(searchResults.Select(SearchResultModel));
    }

    [HttpGet("search/persons/{query}", Name = nameof(SearchPersonsAsync))]
    public async Task<IActionResult> SearchPersonsAsync(string query)
    {
        var searchResults = await _searchService.SearchPersonNameAsync(query);
        return Ok(searchResults.Select(SearchResultModel));
    }

    private SearchResultModel SearchResultModel(SearchResultDTO searchresult)
    {
        // If the title is null, return null (avoiding null reference exceptions)
        if (searchresult == null)
        {
            return null;
        }
        // Map TitleBasic entity properties to TitleBasicModel properties
        var model = searchresult.Adapt<SearchResultModel>();

        // Generate URL for accessing details of the current movie and add to the model
        model.Url = GetUrl(nameof(SearchAsync), new { searchresult.Name });

        return model;
    }

}
