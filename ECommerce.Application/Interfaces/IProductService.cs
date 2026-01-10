using ECommerce.Application.DTOs.Product;

namespace ECommerce.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDetailsResponseDto> CreateAsync(ProductRequestDto dto);
        Task<List<ProductListResponseDto>> GetAllAsync();
        Task<ProductDetailsResponseDto?> GetByIdAsync(int id);
    }
}
