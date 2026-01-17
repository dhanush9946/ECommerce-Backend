using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductRepository:IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                        .Where(p => p.IsActive)
                        .ToListAsync();
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                         .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> SearchAsync(
                            string? search,
                            string? category,
                            string? brand, 
                            string? gender,
                            int page,
                            int pageSize,
                            string? sort)
        {
            IQueryable<Product> query = _context.Products;

            query = query.Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()));

            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category.ToLower() == category.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(brand))
            {
                query = query.Where(p => p.Brand.ToLower() == brand.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(gender))
            {
                query = query.Where(p =>
                    p.Gender.ToLower() == gender.ToLower());
            }




            query = sort switch
            {
                "priceAsc"  => query.OrderBy(p=>p.Price),
                "priceDesc" => query.OrderByDescending(p=>p.Price),
                "newest"    => query.OrderByDescending(p=>p.CreatedAt),
                 _          => query.OrderBy(p=>p.Id)
            };


            //pagination
            int skip = (page - 1) * pageSize;

           return await query.Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
