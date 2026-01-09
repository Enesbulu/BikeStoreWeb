namespace BikeStoreWeb.Core.Entities
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; }

        public string ShippingAddress { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

    }
}
