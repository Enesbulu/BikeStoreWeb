using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Responses;

namespace BikeStoreWeb.Core.Interfaces
{
    public interface IShoppingCartService
    {
        //Bir müşterinin sepetini getirir.
        ServiceResponse<List<ShoppingCartItemDto>> GetCartByCustomerId(int customerId);
        //Sepete ürün ekler veya miktar arttırır.
        ServiceResponse<bool> AddToCart(AddToCartDto addToCartDto);

        //Sepetten tek bir satırı siler
        ServiceResponse<bool> RemoveFromCart(int id);

        //Sipariş sonrası sepeti tamamen boşaltır.
        ServiceResponse<bool> ClearCart(int customerId, int cartId);
    }
}
