

namespace ECommerce.Domain.Entities
{
    public class Product
    {
          public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Category { get; set; } = null!;

        public decimal Price { get; set; }

      
        public int Stock { get; set; }

       
        public int MaxOrderQuantity { get; set; } = 5;

        public bool IsActive { get; set; }

        public string Gender { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
