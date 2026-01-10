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
            /* response = new ServiceResponse<OrderDto>();

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
            return response;*/


            var response = new ServiceResponse<OrderDto>();
            List<OrderItem> finalOrderItems = new List<OrderItem>();
            bool sourceIsDb = false;

            //DB'deki Sepeti Kontrol Et (Server-Side)
            var dbCartItem = _dbContext.ShoppingCartItems
                .Include(c => c.Product)
                .Where(c => c.CustomerId == checkoutDto.CustomerId)
                .ToList();
            if (dbCartItem != null && dbCartItem.Any())
            {
                //Db'den gelen sepeti kullan
                sourceIsDb = true;
                foreach (var item in dbCartItem)
                {
                    finalOrderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Product.Price
                    });
                }

            }
            else
            {
                if (checkoutDto.OrderItems != null && checkoutDto.OrderItems.Any())
                {


                    //Db boş ise Checkout'tan gelen sepeti kullan
                    foreach (var itemDto in checkoutDto.OrderItems)
                    {
                        var product = _dbContext.Products.Find(itemDto.ProductId);
                        if (product != null)
                        {
                            finalOrderItems.Add(new OrderItem
                            {
                                ProductId = itemDto.ProductId,
                                Quantity = itemDto.Quantity,
                                Price = product.Price
                            });
                        }
                    }
                }

            }

            //Hâlâ Sepet Boş mu?
            if (!finalOrderItems.Any())
            {
                response.Success = false;
                response.Message = "Sepetiniz boş, sipariş oluşturulamadı.";
                return response;
            }


            //Yeni Sipariş Oluşturma
            var newOrder = new Order
            {
                CustomerId = checkoutDto.CustomerId,
                OrderDate = DateTime.Now,
                Status = "Hazırlanıyor",
                OrderNumber = "ORM-" + new Random().Next(1000, 9999) + "-" + DateTime.Now.ToString("mmss"),
                ShippingAddress = checkoutDto.ShippingAddress,
                TotalAmount = finalOrderItems.Sum(itm => itm.Quantity * itm.Price),
                OrderItems = finalOrderItems
            };

            //Veritabanına Kaydetme
            _dbContext.Orders.Add(newOrder);

            //Eğer veriyi DB sepetinden çektiysek, işlem bitince orayı temizleme
            if (sourceIsDb)
            {
                //Sadece Db'den gelen sepet kullanıldıysa sepeti temizle
                _dbContext.ShoppingCartItems.RemoveRange(dbCartItem);
            }

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Sipariş oluşturulurken bir hata oluştu." + ex.InnerException.Message ?? ex.Message;
                return response;
            }

            response.Data = new OrderDto
            {
                Id = newOrder.Id,
                OrderNumber = newOrder.OrderNumber,
                Status = newOrder.Status,
                OrderDate = newOrder.OrderDate,
                TotalAmount = newOrder.TotalAmount,
                Items = newOrder.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    //ProductName = _dbContext.Products.Find(oi.ProductId)?.Name ?? "Bilinmeyen Ürün",
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };

            response.Success = true;
            response.Message = $"Siparişiniz alındı. Sipariş no: {newOrder.OrderNumber}";
            return response;



        }



        public ServiceResponse<List<OrderDto>> GetOrderByCustomerId(string customerId)
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
