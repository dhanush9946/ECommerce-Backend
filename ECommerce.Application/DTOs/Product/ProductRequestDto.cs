

namespace ECommerce.Application.DTOs.Product
{
    public class ProductRequestDto
    {
        public string Name { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public string Category { get; set; } = null!;
    public decimal Price { get; set; }

    public int Stock { get; set; }
    public int MaxOrderQuantity { get; set; }

    public string Status { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Image { get; set; } = null!;
    }
}
