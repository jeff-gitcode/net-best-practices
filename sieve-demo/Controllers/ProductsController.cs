using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Exceptions;
using Sieve.Models;
using Sieve.Services;

namespace sieve_demo.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly SieveProcessor _sieveProcessor;
    private readonly ApplicationDbContext _context;

    public ProductsController(SieveProcessor sieveProcessor, ApplicationDbContext context)
    {
        _sieveProcessor = sieveProcessor;
        _context = context;
    }

    [HttpGet(Name = "GetProducts")]
    public IActionResult Get([FromQuery] SieveModel sieveModel)
    {
        try
        {
            var products = _context.Products.AsNoTracking();

            products = _sieveProcessor.Apply(sieveModel, products);

            return Ok(products.ToList());
        }
        catch (SieveException ex)
        {
            // Handle exception here
            return BadRequest(ex.Message);
        }
    }
}
