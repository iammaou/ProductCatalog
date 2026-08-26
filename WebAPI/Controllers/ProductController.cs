using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService service;

        public ProductController(ProductService service)
        {
            this.service = service;
        }

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
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDTO productDTO)
        {
            var result = await service.AddCategoryAsync(productDTO);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> RemoveProduct(Guid id)
        {
            var result = await service.RemoveProductAsync(id);

            return Ok();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductDTO productDTO)
        {
            var result = await service.UpdateCategoryAsync(id, productDTO);

            return Ok(result);
        }
    }
}
