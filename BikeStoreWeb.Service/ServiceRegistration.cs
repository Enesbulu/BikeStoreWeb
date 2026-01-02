using BikeStoreWeb.Core.Interfaces;
using BikeStoreWeb.Core.Services;
using BikeStoreWeb.Service.Services;

using Microsoft.Extensions.DependencyInjection;

namespace BikeStoreWeb.Service
{
    public static class ServiceRegistration
    {
        /// <summary>
        /// IOC için Tüm servis kayıtlarını burada toplayacağız.
        /// </summary>
        /// <param name="services"></param>
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }
    }
}
