using BikeStoreWeb.Data.Context;
using BikeStoreWeb.Service.DTOs;
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

        public List<ProductDto> GetAllProducts()
        {
            var products = _dbContext.Products //Db'den manuel çekme
                .Include(p => p.Category)    //Category ismini almak için join yapıyoruz.
                .Select(p => new ProductDto //manuel olarak map işlemi yapıyoruz.
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    CategoryName = p.Category.Name,
                }).ToList();
            return products;
        }

        public ProductDto GetProductById(int id)
        {
            var product = _dbContext.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);
            
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryName = product.Category.Name,
            };
        }
    }
}
