using ECommerce.API.DTOs;
using ECommerce.Application.DTOs.Product;

namespace ECommerce.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDetailsResponseDto> CreateAsync(ProductRequestDto dto);

        Task UpdateAsync(int id, UpdateProductDto dto);
        Task DeleteAsync(int id);


        Task<List<ProductListResponseDto>> GetAllAsync();
        Task<ProductDetailsResponseDto?> GetByIdAsync(int id);
        Task<UpdateProductDto?> GetByIdAsyncAdmin(int id); 
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
