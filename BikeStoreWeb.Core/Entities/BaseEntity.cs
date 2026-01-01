namespace BikeStoreWeb.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
