

namespace ECommerce.Domain.Entities
{
    public class Wishlist
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Product? Product { get; set; }
    }
}
