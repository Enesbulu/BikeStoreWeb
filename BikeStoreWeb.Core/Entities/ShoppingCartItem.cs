namespace BikeStoreWeb.Core.Entities
{
    public class ShoppingCartItem : BaseEntity
    {
        public string UserId { get; set; }  // Giriş yapan kullanıcı veya SessionId
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }   // Kaç tane ekledi?
    }
}
