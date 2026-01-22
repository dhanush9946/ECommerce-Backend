

namespace ECommerce.Application.DTOs.Order
{
    public class AdminOrderResponseDto
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }

        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public string ShippingAddress { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public List<AdminOrderItemDto> Items { get; set; } = new();
    }
}
