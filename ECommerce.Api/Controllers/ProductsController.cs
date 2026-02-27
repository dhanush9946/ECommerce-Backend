using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    
 
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(new { message = "CI/CD is working!", data = await _service.GetAllAsync() });

           // return Ok(await _service.GetAllAsync());
        }
        catch(ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }
        catch(ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> Search(
        [FromQuery] string? search,
        [FromQuery] string? category,
        [FromQuery] string? brand,
        [FromQuery] string? gender,
        [FromQuery] int page=1,
        [FromQuery] int pageSize=10,
        [FromQuery] string? sort = null)
    {
        try
        {
            var result = await _service.SearchAsync(search, category, brand, gender, page, pageSize, sort);
            return Ok(result);
        }
        catch(ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
