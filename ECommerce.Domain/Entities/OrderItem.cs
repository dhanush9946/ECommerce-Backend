

namespace ECommerce.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
