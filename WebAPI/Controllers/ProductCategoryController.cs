using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController(IProductCategoryService service) : ControllerBase
    {
        private readonly IProductCategoryService service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await service.GetAllCategoriesAsync();
            
            return Ok(categories);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await service.GetCategoryAsync(id);
            
            return category is null ? NotFound() : Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(ProductCategoryDTO addProductCategoryDTO)
        {
            var category = await service.AddCategoryAsync(addProductCategoryDTO);

            return CreatedAtAction(nameof(GetCategory), new {id = category.Id}, category);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategory(Guid id, ProductCategoryDTO productCategoryDTO)
        {
            var category = await service.UpdateCategoryAsync(id, productCategoryDTO);

            return category is null ? NotFound() : Ok(category);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await service.RemoveCategoryAsync(id);

            return category switch
            {
                ProductCategoryDeleteResult.NotFound => NotFound(),
                ProductCategoryDeleteResult.hasProducts => Conflict("Cannot delete category that has products."),
                ProductCategoryDeleteResult.Success => NoContent(),
                _ => StatusCode(500)
            };
        }
    }
}
