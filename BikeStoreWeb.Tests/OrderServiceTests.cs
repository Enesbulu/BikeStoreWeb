using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Entities;
using BikeStoreWeb.Data.Context;
using BikeStoreWeb.Service.Services;
using Microsoft.EntityFrameworkCore;

namespace BikeStoreWeb.Tests
{
    public class OrderServiceTests
    {

        private BikeStoreDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<BikeStoreDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new BikeStoreDbContext(options);
            return context;
        }
        [Fact]
        public void CreateOrder_Should_CreateOrder_And_ClearCart_When_Successful()
        {
            //1.Arrange-Hazırlık
            var context = GetInMemoryDbContext();


            //--- Sahte veri girişi

            //Ürün ekleme
            var product = new Product
            {
                Name = "Test Bike",
                Price = 1000,
                Description = "test",
                CategoryId = 1
            };
            context.Products.Add(product);

            //Bir sepet öğesi
            var cartItem = new ShoppingCartItem
            {
                ProductId = 1,
                CustomerId = 1,
                Quantity = 2
            };
            context.ShoppingCartItems.Add(cartItem);

            context.SaveChanges();

            //Servis Oluşturma
            var orderService = new OrderService(context);

            //Test girdisi
            var checkoutDto = new CheckoutDto { CustomerId = 1 };

            //--- ACT-Eylem
            //Metod çalıştırma
            var result = orderService.CreateOrder(checkoutDto);

            //--- Assert-Doğrulama
            //Sonuç başarılı mı kontrolü
            Assert.True(result.Success, "Sipariş oluşturma işlemi başarısız oldu.");

            //Sipariş numarası Dolu mu kontrolü
            Assert.NotNull(result.Data.OrderNumber);

            //Veritabanunda sipariş var mı kontrolü
            var orderInDb = context.Orders.FirstOrDefault();
            Assert.NotNull(orderInDb);
            Assert.Equal(1, orderInDb.CustomerId);

            //Fiyat hesaplaması doğru mu kontrolü
            Assert.Equal(2000, orderInDb.TotalAmount);

            //Sepet temizlendi mi kontrolü
            var cartItemsCount = context.ShoppingCartItems.Count();
            Assert.Equal(0, cartItemsCount);
        }

        [Fact]
        public void CreateOrder_Should_ReturnError_When_CartIsEmpty()
        {
            //--- Hazırlık 
            var context = GetInMemoryDbContext();

            //Sepet boş bırakılıyor
            var orderService = new OrderService(context);
            var checkOutDto = new CheckoutDto
            {
                CustomerId = 99,    //olmayan bir müşteri
            };

            //---- Eylem,
            var result = orderService.CreateOrder(checkOutDto);

            //---- Doğrulama
            Assert.False(result.Success);
            Assert.Equal("Sepetiniz boş, sipariş oluşturulamadı.", result.Message);

        }
    }
}
