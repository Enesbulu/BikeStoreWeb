namespace BikeStoreWeb.Core.Entities
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public decimal Price { get; set; }  // Satın alındığı andaki fiyat
        public int Quantity { get; set; }
    }
}
