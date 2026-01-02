using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Entities;
using BikeStoreWeb.Core.Responses;
using BikeStoreWeb.Core.Services;
using BikeStoreWeb.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeStoreWeb.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly BikeStoreDbContext _dbContext;

        //Constructor Injection yapıyoruz.
        public ProductService(BikeStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ServiceResponse<List<ProductDto>> GetAllProducts()
        {
            var response = new ServiceResponse<List<ProductDto>>();
            var products = _dbContext.Products
                .Include(p => p.Category)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    CategoryName = p.Category.Name
                }).ToList();
            response.Data = products;
            response.Message = "Ürünler Başarıyla Listelendi.";
            return response;
        }

        public ServiceResponse<ProductDto> GetProductById(int id)
        {
            var response = new ServiceResponse<ProductDto>();
            var product = _dbContext.Products
                .Include(p => p.Id)
                .FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                response.Success = false;
                response.Message = "Ürün Bulunamadı!";
                return response;
            }

            response.Data = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryName = product.Category.Name
            };
            return response;
        }

        public ServiceResponse<ProductDto> CreateProduct(CreateProductDto createProductDto)
        {
            var response = new ServiceResponse<ProductDto>();
            var product = new Product
            {
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Description = createProductDto.Description,
                CategoryId = createProductDto.CategoryId
            };
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            response.Data = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryName = ""
            };
            response.Message = "ürün Başarıyla Eklendi.";
            response.Success = true;
            return response;
        }

        public ServiceResponse<bool> UpdateProduct(UpdateProductDto updateProductDto)
        {
            var response = new ServiceResponse<bool>();
            var product = _dbContext.Products.Find(updateProductDto.Id);
            if (product == null)
            {
                response.Success = false;
                response.Message = "Güncellenecek Ürün Bulunamadı.";
                response.Data = false;
                return response;
            }

            product.Name = updateProductDto.Name;
            product.Price = updateProductDto.Price;
            product.Description = updateProductDto.Description;
            product.CategoryId = updateProductDto.CategoryId;

            _dbContext.SaveChanges();
            response.Data = true;
            response.Message = "Ürün Başarıyla Güncellendi.";
            return response;
        }


        public ServiceResponse<bool> DeleteProduct(int id)
        {
            var response = new ServiceResponse<bool>();
            var product = _dbContext.Products.Find(id);

            if (product == null)
            {
                response.Success = false;
                response.Message = "Silinecek ürün bulunamadı.";
                response.Data = false;
                return response;

            }

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            response.Data = true;
            response.Message = "Ürün başarıyla silindi.";
            return response;

        }



    }
}
