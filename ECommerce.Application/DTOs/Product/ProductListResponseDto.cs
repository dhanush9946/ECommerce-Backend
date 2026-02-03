

namespace ECommerce.Application.DTOs.Product
{
    public class ProductListResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }

        public bool InStock { get; set; }
        public string Description { get; set; } = null!;

        public string Image { get; set; } = null!;

    }
}
