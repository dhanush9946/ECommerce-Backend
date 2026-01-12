

namespace ECommerce.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Product? Product { get; set; }
    }
}
