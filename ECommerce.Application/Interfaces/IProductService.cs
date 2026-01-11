using ECommerce.Application.DTOs.Product;

namespace ECommerce.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDetailsResponseDto> CreateAsync(ProductRequestDto dto);
        Task<List<ProductListResponseDto>> GetAllAsync();
        Task<ProductDetailsResponseDto?> GetByIdAsync(int id);
        Task<List<ProductListResponseDto>> SearchAsync(
            string? search,
            string? category,
            string? brand,
            string? gender,
            int page,
            int pageSize,
            string? sort
        );
    }
}
