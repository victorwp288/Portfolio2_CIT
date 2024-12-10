using Microsoft.AspNetCore.Mvc;
using Mapster;
using WebServiceLayer.Models.Persons;
using DataAcessLayer;
using DataAcessLayer.Repositories.Implementations;
using DataAcessLayer.Repositories.Interfaces;
using DataAcessLayer.Context;

namespace WebServiceLayer.Controllers.Persons;

[ApiController]
[Route("api/persons")]
public class PersonsController : BaseController
{
    IDataService _dataService;
    private DataService _service;
    private readonly ImdbContext _context;
    private readonly LinkGenerator _linkGenerator;
    public PersonsController(IDataService dataService, ImdbContext context, LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _dataService = dataService;
        _service = new DataService(_context);
        Console.WriteLine(_service);
    }


    [HttpGet("{nconst}")]
    public async Task<IActionResult> GetPersonDetailsByNconst(string nconst)
    {
        var personDet = await _service.GetNameBasicByNconst(nconst);
        Console.WriteLine(personDet.PrimaryName);
        if (personDet != null)
        {
            var model = personDet.Adapt< PersonDetailsModel>();
            return Ok(model);
        }
        else
        {
            return NotFound();
        }
    }
}