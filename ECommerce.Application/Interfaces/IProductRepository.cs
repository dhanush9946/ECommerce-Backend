using ECommerce.Domain.Entities;


namespace ECommerce.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task UpdateAsync(Product product);

        Task<List<Product>> SearchAsync(
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
