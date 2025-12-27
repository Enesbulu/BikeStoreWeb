namespace BikeStoreWeb.Core.Entities
{
    public class Category : BaseEntity
    {
        public string Name  { get; set; }
        public string Description { get; set; }

        // Bir kategoride birden fazla ürün olabilir (One-to-Many)
        public ICollection<Product> Products { get; set; }
    }
}
