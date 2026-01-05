namespace BikeStoreWeb.Core.DTOs
{
    public class AddToCartDto
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
    }
}
