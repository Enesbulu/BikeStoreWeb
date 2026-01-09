using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Entities;
using BikeStoreWeb.Core.Interfaces;
using BikeStoreWeb.Core.Responses;
using BikeStoreWeb.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeStoreWeb.Service.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly BikeStoreDbContext _dbContext;

        public ShoppingCartService(BikeStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ServiceResponse<List<ShoppingCartItemDto>> GetCartByCustomerId(string customerId)
        {
            var response = new ServiceResponse<List<ShoppingCartItemDto>>();
            var cartItem = _dbContext.ShoppingCartItems
                .Include(c => c.Product)
                .Where(c => c.CustomerId == customerId)
                .Select(c => new ShoppingCartItemDto
                {
                    Id = c.Id,
                    ProductId = c.ProductId,
                    ProductName = c.Product.Name,
                    UnitPrice = c.Product.Price,
                    Quantity = c.Quantity,
                    TotalPrice = c.Quantity * c.Product.Price,
                }).ToList();

            response.Data = cartItem;
            response.Message = "Sepet Listelendi.";
            return response;
        }

        public ServiceResponse<bool> AddToCart(AddToCartDto addToCartDto)
        {
            var response = new ServiceResponse<bool>();
            //Ürün var mı kontrolü
            var product = _dbContext.Products.Find(addToCartDto.ProductId);
            if (product == null)
            {
                response.Success = false;
                response.Message = "Eklemek istenen  ürün bulunamadı.";
                return response;
            }

            //Ürün müşteri sepetinde var mı kontrolü
            var existingItem = _dbContext.ShoppingCartItems
                .FirstOrDefault(c => c.CustomerId == addToCartDto.CustomerId
                && c.ProductId == addToCartDto.ProductId);

            if (existingItem != null)
            {
                //varsa miktarı arttır
                existingItem.Quantity += addToCartDto.Quantity;
                response.Message = "Ürün miktarı güncellendi.";
            }
            else
            {
                var newItem = new ShoppingCartItem
                {
                    CustomerId = addToCartDto.CustomerId,
                    ProductId = addToCartDto.ProductId,
                    Quantity = addToCartDto.Quantity,
                };
                _dbContext.ShoppingCartItems.Add(newItem);
                response.Message = "Ürün sepete eklendi.";
            }

            _dbContext.SaveChanges();
            response.Data = true;
            return response;

        }

        public ServiceResponse<bool> RemoveFromCart(int id)
        {
            var response = new ServiceResponse<bool>();
            var item = _dbContext.ShoppingCartItems.Find(id);

            if (item == null)
            {
                response.Success = false;
                response.Message = "sepet öğresi bulunamadı.";
                return response;
            }

            _dbContext.ShoppingCartItems.Remove(item);
            _dbContext.SaveChanges();

            response.Data = true;
            response.Message = "Ürün sepetten çıkarıldı.";
            return response;
        }

        public ServiceResponse<bool> ClearCart(string customerId, int cartId)
        {
            var response = new ServiceResponse<bool>();
            var items = _dbContext.ShoppingCartItems
                .Where(c => c.CustomerId == customerId)
                .ToList();
            if (items.Any())
            {
                _dbContext.ShoppingCartItems.RemoveRange(items);
                _dbContext.SaveChanges();
            }

            response.Data = true;
            response.Message = "Sepet temizlendi.";

            return response;
        }


    }
}
