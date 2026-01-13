using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDetailsResponseDto> CreateAsync(ProductRequestDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Brand = dto.Brand,
                Category = dto.Category,
                Price = dto.Price,
                Stock = dto.Stock,
                MaxOrderQuantity = dto.MaxOrderQuantity,
                Gender = dto.Gender,
                Description = dto.Description,
                ImageUrl = dto.Image,
                IsActive = dto.Status.ToLower() == "active"
            };

            var result = await _repository.AddAsync(product);

            return MapToDetailsDto(result);
        }

        public async Task<List<ProductListResponseDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();

            return products.Select(p => new ProductListResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand,
                Category = p.Category,
                Price = p.Price,
                InStock = p.Stock > 0,
                Image = p.ImageUrl,
                //Status = p.IsActive ? "active" : "inactive"
            }).ToList();
        }

        public async Task<ProductDetailsResponseDto?> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return null;

            return MapToDetailsDto(product);
        }

        private static ProductDetailsResponseDto MapToDetailsDto(Product product)
        {
            return new ProductDetailsResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                AvailableStock = product.Stock,
                MaxOrderQuantity = product.MaxOrderQuantity,
                Gender = product.Gender,
                Description = product.Description,
                Image = product.ImageUrl,
                //Status = product.IsActive ? "active" : "inactive"
            };
        }

        public async Task<List<ProductListResponseDto>> SearchAsync(
            string? search,
            string? category,
            string? brand,
            string? gender,
            int page,
            int pageSize,
            string? sort
        )
        {
            var products = await _repository.SearchAsync(
                      search, category, brand,gender,page,pageSize,sort);

            return products.Select(p => new ProductListResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand,
                Category = p.Category,
                Price = p.Price,
                InStock = p.Stock > 0,
                Image = p.ImageUrl,
                //Status = p.IsActive ? "active" : "inactive"
            }).ToList();
        }

    }
}
