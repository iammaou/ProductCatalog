using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService service) : ControllerBase
    {
        private readonly IProductService service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters query)
        {
            var result = await service.GetAllProductsAsync(query);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var result = await service.GetProductAsync(id);
            return result is null ? NotFound() : Ok(new ProductDTO { Id = result.Id, Name = result.Name, Price = result.Price, IsActive = result.IsActive, StockQuantity = result.StockQuantity, CreatedAt = result.CreatedAt, CategoryId = result.CategoryId});
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDTO productDTO)
        {
            var result = await service.AddProductAsync(productDTO);
            return CreatedAtAction(nameof(GetProduct), new { id = result.Id}, result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> RemoveProduct(Guid id)
        {
            var result = await service.RemoveProductAsync(id);

            return result is null ? NotFound() : Ok();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductDTO productDTO)
        {
            var result = await service.UpdateProductAsync(id, productDTO);

            return result is null ? NotFound() : Ok(result);
        }
    }
}
