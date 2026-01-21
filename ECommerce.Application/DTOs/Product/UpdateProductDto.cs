namespace ECommerce.API.DTOs
{
    public class UpdateProductDto
    {
        public string? Name { get; set; } 
        public string? Brand { get; set; } 
        public string? Category { get; set; } 
        public string? Gender { get; set; }

        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public int? MaxOrderQuantity { get; set; }

        public bool? IsActive { get; set; }
        public string? Description { get; set; } 
        public string? Image { get; set; }
    }
}
