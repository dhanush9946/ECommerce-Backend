

namespace ECommerce.Application.DTOs.Cart
{
    public class UpdateCartQuantityDto
    {
        public int ProductId { get; set; }
        public string? Action { get; set; }
    }
}
