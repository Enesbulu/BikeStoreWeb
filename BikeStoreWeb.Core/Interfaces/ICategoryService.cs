using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Responses;

namespace BikeStoreWeb.Core.Interfaces
{
    public interface ICategoryService
    {
        ServiceResponse<List<CategoryDto>> GetAllCategories();
        ServiceResponse<CategoryDto> GetCategoryById(int id);
        ServiceResponse<CategoryDto> CreateCategory(CreateCategoryDto createCategoryDto);
        ServiceResponse<bool> UpdateCategorry(UpdateCategoryDto updateCategoryDto);
        ServiceResponse<bool> DeleteCategory(int id);
    }
}
