using BikeStoreWeb.Service.DTOs;

namespace BikeStoreWeb.Service.Services
{
    public interface IProductService
    {
        List<ProductDto> GetAllProducts();  //Tüm ürünleri product ile getir
        ProductDto GetProductById(int id);  //bir tane ürünü product ile getir.
    }
}
