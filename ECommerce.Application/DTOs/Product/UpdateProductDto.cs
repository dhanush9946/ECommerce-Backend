namespace ECommerce.API.DTOs
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Gender { get; set; } = null!;

        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int MaxOrderQuantity { get; set; }

        public bool IsActive { get; set; }
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
    }
}
