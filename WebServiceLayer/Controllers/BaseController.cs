using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace WebServiceLayer.Controllers;


// BaseController is an abstract base class that other API controllers can inherit from
// Provides helper methods for URL generation and pagination
[ApiController]
public class BaseController : ControllerBase
{
    private readonly LinkGenerator _linkGenerator;

    // Constructor that injects LinkGenerator via dependency injection
    public BaseController(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    // Generates a full URL based on a named route and the specified arguments
    // linkName is the route name, and args contains any parameters required for the URL
    protected string? GetUrl(string linkName, object args)
    {
        return _linkGenerator.GetUriByName(
            HttpContext,
            linkName,
            args);
            
    }


    protected string? GetLink(string linkName, int page, int pageSize)
    {
        return GetUrl(linkName, new { page, pageSize });
    }

    // Creates a paginated response object, containing links and metadata for paging
    protected object CreatePaging<T>(string linkName, int page, int pageSize, int total, IEnumerable<T?> items)
    {
        const int MaxPageSize = 25; // Limits the maximum page size to 25
        pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize; // Ensures pageSize does not exceed MaxPageSize

        // Calculate the total number of pages based on total items and page size
        var numberOfPages = (int)Math.Ceiling(total / (double)pageSize);

        // Generate current page link using GetLink
        var curPage = GetLink(linkName, page, pageSize);

        // Generate the next page link, or null if on the last page
        var nextPage = page < numberOfPages - 1
            ? GetLink(linkName, page + 1, pageSize)
            : null;

        // Generate the previous page link, or null if on the first page
        var prevPage = page > 0
            ? GetLink(linkName, page - 1, pageSize)
            : null;

        // Construct the paginated response object with navigation links and metadata
        var result = new
        {
            CurPage = curPage,      
            NextPage = nextPage,    
            PrevPage = prevPage,    
            NumberOfItems = total,  
            NumberPages = numberOfPages, 
            Items = items           
        };
        return result; 
    }
}
