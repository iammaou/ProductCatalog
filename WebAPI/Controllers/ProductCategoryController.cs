using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly ProductCategoryService service;

        public ProductCategoryController(ProductCategoryService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await service.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await service.GetCategoryAsync(id);
            
            return category is null ? NotFound() : Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(AddProductCategoryDTO addProductCategoryDTO)
        {
            var category = await service.AddCategoryAsync(addProductCategoryDTO);

            return Ok(category);
        }
    }
}
