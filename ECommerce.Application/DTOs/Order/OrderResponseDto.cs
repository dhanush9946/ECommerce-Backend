
namespace ECommerce.Application.DTOs.Order
{
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
