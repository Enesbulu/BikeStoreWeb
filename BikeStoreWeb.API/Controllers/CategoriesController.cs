using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Interfaces;
using BikeStoreWeb.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace BikeStoreWeb.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")] // api/v1/categories
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult<ServiceResponse<List<CategoryDto>>> GetAll()
        {
            var result = _categoryService.GetAllCategories();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<ServiceResponse<CategoryDto>> GetById(int id)
        {
            var response = _categoryService.GetCategoryById(id);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [HttpPost]
        public ActionResult<ServiceResponse<CategoryDto>> Create(CreateCategoryDto createCategoryDto)
        {
            var response = _categoryService.CreateCategory(createCategoryDto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public ActionResult<ServiceResponse<bool>> Update(int id, UpdateCategoryDto updateCategoryDto)
        {
            if (id != updateCategoryDto.Id)
                return BadRequest(new ServiceResponse<bool>()
                {
                    Success = false,
                    Message = "Id Uyuşmazlığı"
                });

            var response = _categoryService.UpdateCategorry(updateCategoryDto);

            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public ActionResult<ServiceResponse<bool>> Delete(int id)
        {
            var response = _categoryService.DeleteCategory(id);
            if (!response.Success) return NotFound(Response);
            return Ok(response);
        }
    }
}
