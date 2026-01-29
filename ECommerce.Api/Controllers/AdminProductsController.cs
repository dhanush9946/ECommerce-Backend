using ECommerce.API.DTOs;
using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [ApiController]
    [Route("api/admin/products")]
    [Authorize(Roles = "Admin")]
    public class AdminProductsController:ControllerBase
    {
        private readonly IProductService _productService;
        public AdminProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductRequestDto dto)
        {
            return Ok(await _productService.CreateAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductDto dto)
        {
            await _productService.UpdateAsync(id, dto);
            return Ok("Product updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return Ok("Product deleted");
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] AdminProductQueryDto query)
        {
            var result = await _productService.GetAdminProductsAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            return Ok(await _productService.GetByIdAsyncAdmin(id));
        }

    }
}
