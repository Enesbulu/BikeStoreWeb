using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Entities;
using BikeStoreWeb.Core.Interfaces;
using BikeStoreWeb.Core.Responses;
using BikeStoreWeb.Data.Context;

namespace BikeStoreWeb.Service.Services
{
    internal class CategoryService : ICategoryService
    {
        private readonly BikeStoreDbContext _context;

        public CategoryService(BikeStoreDbContext context)
        {
            _context = context;
        }

        public ServiceResponse<List<CategoryDto>> GetAllCategories()
        {
            var response = new ServiceResponse<List<CategoryDto>>();
            var category = _context.Categories
                .Select(c =>
                new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                }).ToList();
            response.Data = category;
            response.Message = "Kategoriler Listelendi.";
            return response;
        }
        public ServiceResponse<CategoryDto> GetCategoryById(int id)
        {
            var response = new ServiceResponse<CategoryDto>();
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                response.Success = false;
                response.Message = "Kategori Bulunamadı.";
                return response;
            }

            response.Data = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
            };
            return response;

        }
        public ServiceResponse<CategoryDto> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            var response = new ServiceResponse<CategoryDto>();
            var category = new Category
            {
                Name = createCategoryDto.Name,
                Description = createCategoryDto.Description,
            };
            _context.Categories.Add(category);
            _context.SaveChanges();
            response.Data = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
            };
            response.Message = "Kategory başarıyla oluşturuldu.";
            return response;
        }

        public ServiceResponse<bool> UpdateCategorry(UpdateCategoryDto updateCategoryDto)
        {
            var response = new ServiceResponse<bool>();
            var category = _context.Categories.Find(updateCategoryDto.Id);
            if (category == null)
            {
                response.Success = false;
                response.Message = "Güncellenecek kategori bulunamadı.";
                return response;
            }
            ;

            category.Name = updateCategoryDto.Name;
            category.Description = updateCategoryDto.Description;
            _context.SaveChanges();
            response.Data = true;
            response.Message = "Kategori eklendi.";
            return response;
        }

        public ServiceResponse<bool> DeleteCategory(int id)
        {
            var response = new ServiceResponse<bool>();
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                response.Success = false;
                response.Message = "Silinecek kategory bulunamadı.";
                return response;

            }
            _context.Categories.Remove(category);
            _context.SaveChanges();

            response.Data = true;
            response.Message = "Kategori silindi.";
            return response;
        }
    }
}
