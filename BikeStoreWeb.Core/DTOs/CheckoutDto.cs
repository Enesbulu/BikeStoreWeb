namespace BikeStoreWeb.Core.DTOs
{
    public class CheckoutDto
    {
        public string? CustomerId { get; set; }
        public string ShippingAddress { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
