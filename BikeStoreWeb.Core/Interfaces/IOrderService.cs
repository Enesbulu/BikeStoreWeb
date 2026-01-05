using BikeStoreWeb.Core.DTOs;
using BikeStoreWeb.Core.Responses;

namespace BikeStoreWeb.Core.Interfaces
{
    public interface IOrderService
    {
        ServiceResponse<OrderDto> CreateOrder(CheckoutDto checkoutDto);
        ServiceResponse<List<OrderDto>> GetOrderByCustomerId(int customerId);
    }
}
