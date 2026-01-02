using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Responses;

namespace BikeStoreWeb.Core.Services
{
    public interface IProductService
    {
        // List<ProductDto> yerine:
        ServiceResponse<List<ProductDto>> GetAllProducts();

        // ProductDto yerine:
        ServiceResponse<ProductDto> GetProductById(int id);

        ServiceResponse<ProductDto> CreateProduct(CreateProductDto createProductDto);

        // Void olanlar da artık bilgi dönmeli (Başarılı/Başarısız mesajı için)
        ServiceResponse<bool> UpdateProduct(UpdateProductDto updateProductDto);
        ServiceResponse<bool> DeleteProduct(int id);
    }
}
