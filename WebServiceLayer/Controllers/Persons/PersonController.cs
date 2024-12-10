using Microsoft.AspNetCore.Mvc;
using Mapster;
using WebServiceLayer.Models.Persons;
using DataAcessLayer;
using DataAcessLayer.Repositories.Implementations;

namespace WebServiceLayer.Controllers.Persons;

[ApiController]
[Route("api/persons")]
public class PersonsController : BaseController
{
    IDataService _dataService;
    private DataService _service;
    private readonly LinkGenerator _linkGenerator;

    public PersonsController(IDataService dataService, LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _dataService = dataService; 
        Console.WriteLine(_dataService); 
    }

    [HttpGet("{nconst}")]
    public async Task<IActionResult> GetPersonDetailsByNconst(string nconst)
    {
        var personDet = await _dataService.GetNameBasicByNconst(nconst); 
        if (personDet != null)
        {
            var model = personDet.Adapt<PersonDetailsModel>();
            return Ok(model);
        }
        else
        {
            return NotFound();
        }
    }
}