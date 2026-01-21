using ECommerce.API.DTOs;
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
            if (dto.Price <= 0)
                throw new Exception("Price must be greater than zero");

            if (dto.Stock < 0)
                throw new Exception("Stock cannot be negative");

            if (dto.MaxOrderQuantity <= 0)
                throw new Exception("MaxOrderQuantity must be greater than zero");
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

        public async Task UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");

            // ONLY update what is sent
            if (dto.Name != null) product.Name = dto.Name;
            if (dto.Brand != null) product.Brand = dto.Brand;
            if (dto.Category != null) product.Category = dto.Category;
            if (dto.Gender != null) product.Gender = dto.Gender;

            if (dto.Price.HasValue)
                product.Price = dto.Price.Value;

            if (dto.Stock.HasValue)
                product.Stock = dto.Stock.Value;

            if (dto.MaxOrderQuantity.HasValue)
                product.MaxOrderQuantity = dto.MaxOrderQuantity.Value;

            if (dto.IsActive.HasValue)
                product.IsActive = dto.IsActive.Value;

            if (dto.Description != null) product.Description = dto.Description;
            if (dto.Image != null) product.ImageUrl = dto.Image;

            await _repository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            await _repository.DeleteAsync(product);
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
                      search, category, brand, gender, page, pageSize, sort);

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


        public async Task<UpdateProductDto?> GetByIdAsyncAdmin(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return null;

            return MapToDetailsDtoAdmin(product);
        }

        private static UpdateProductDto MapToDetailsDtoAdmin(Product product)
        {
            return new UpdateProductDto
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Stock = product.Stock,
                MaxOrderQuantity = product.MaxOrderQuantity,
                Gender = product.Gender,
                Description = product.Description,
                Image = product.ImageUrl,
                IsActive = product.IsActive

            };
        }
    }

    
}
