

namespace ECommerce.Application.DTOs.Wishlist
{
    public class WishlistResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; } = null!;
    }
}
