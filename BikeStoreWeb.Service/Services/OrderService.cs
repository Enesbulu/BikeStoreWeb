using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Entities;
using BikeStoreWeb.Core.Interfaces;
using BikeStoreWeb.Core.Responses;
using BikeStoreWeb.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BikeStoreWeb.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly BikeStoreDbContext _dbContext;

        public OrderService(BikeStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ServiceResponse<OrderDto> CreateOrder(CheckoutDto checkoutDto)
        {
            var response = new ServiceResponse<OrderDto>();

            //Müşterinin sepetini getirme
            var cartItems = _dbContext.ShoppingCartItems
                .Include(c => c.Product)
                .Where(c => c.CustomerId == checkoutDto.CustomerId)
                .ToList();

            if (cartItems == null || !cartItems.Any())
            {
                response.Success = false;
                response.Message = "Sepetiniz boş, sipariş oluşturulamadı.";
                return response;
            }

            //Yeni sipariş oluşturma
            var newOrder = new Order
            {
                CustomerId = checkoutDto.CustomerId,
                OrderDate = DateTime.Now,
                Status = "Hazırlanıyor",
                OrderNumber = "ORM-" + new Random().Next(1000, 9999) + "-" + DateTime.Now.ToString("mmss"),
                TotalAmount = cartItems.Sum(itm => itm.Quantity * itm.Product.Price)
            };

            //Sipariş Detaylarını ekleme
            newOrder.OrderItems = new List<OrderItem>();
            foreach (var cartItem in cartItems)
            {
                newOrder.OrderItems.Add(
                    new OrderItem
                    {
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Product.Price,
                    });
            }

            //Veritabanına kaydetme
            _dbContext.Orders.Add(newOrder);

            //Sepeti Temizleme
            _dbContext.ShoppingCartItems.RemoveRange(cartItems);

            _dbContext.SaveChanges();

            response.Data = new OrderDto
            {
                Id = newOrder.Id,
                OrderNumber = newOrder.OrderNumber,
                Status = newOrder.Status,
                OrderDate = newOrder.OrderDate,
                TotalAmount = newOrder.TotalAmount,
                Items = newOrder.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductName = cartItems.First(c => c.ProductId == oi.ProductId).Product.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };

            response.Message = $"Siparişiniz alındı. Sipariş no: {newOrder.OrderNumber}";
            return response;
        }

        public ServiceResponse<List<OrderDto>> GetOrderByCustomerId(int customerId)
        {
            var response = new ServiceResponse<List<OrderDto>>();

            var orders = _dbContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate).ToList();

            response.Data = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                Status = o.Status,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Items = o.OrderItems.Select(
                    oi => new OrderItemDto
                    {
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        Price = oi.Price
                    }).ToList(),
            }).ToList();

            response.Message = "Sipariş geçmişi listelendi.";
            return response;
        }
    }
}
