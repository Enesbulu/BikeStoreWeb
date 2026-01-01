using BikeStoreWeb.Core.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace BikeStoreWeb.Data.Context

{
    public static class DbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<BikeStoreDbContext>();
                context.Database.EnsureCreated();   //BD oluştuğundan emin olmak için kullanıldı.

                if (context.Products.Any()) //Eğer içerisinde ürün varsa ekleme
                    return;

                //Kategori eklemesi
                var category = new Category
                {
                    Name = "Mountain Bikes",
                    Description = "Dağ bisikletleri kategorisi"
                };

                //Ürün Eklemesi
                var product = new Product
                {
                    Name = "Trek Fuel EX 5",
                    Description = "Full süspansiyon dağ bisikleti",
                    Price = 1200.50m,
                    Category = category,
                };

                context.Categories.Add(category);
                context.Products.Add(product);
                context.SaveChanges();
            }
        }
    }
}
