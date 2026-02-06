using ECommerce.Application.DTOs.Product;
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

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        //public async Task SoftDeleteAsync(Product product)
        //{
        //    product.IsActive = false;      
        //    await _context.SaveChangesAsync();
        //}

        public async Task SoftDeleteAsync(Product product)
        {
            Console.WriteLine($"Before: {product.IsActive}");
            product.IsActive = !product.IsActive;
            Console.WriteLine($"After: {product.IsActive}");
            await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetByIdAdminAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
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

        
        //User search and filter ...
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
                var lowerSearch = search.ToLower();

                query = query
                    .Where(p =>
                        p.Name.ToLower().Contains(lowerSearch) ||
                        p.Brand.ToLower().Contains(lowerSearch)
                    )
                    .OrderByDescending(p => p.Name.ToLower().StartsWith(lowerSearch))
                    .ThenBy(p => p.Name);
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




            // Apply sorting ONLY if sort is provided
            if (!string.IsNullOrWhiteSpace(sort))
            {
                query = sort switch
                {
                    "priceAsc" => query.OrderBy(p => p.Price),
                    "priceDesc" => query.OrderByDescending(p => p.Price),
                    "newest" => query.OrderByDescending(p => p.CreatedAt),
                    _ => query
                };
            }
            else if (string.IsNullOrWhiteSpace(search))
            {
                // Default ordering ONLY when NOT searching
                query = query.OrderBy(p => p.Id);
            }



            //pagination
            int skip = (page - 1) * pageSize;

           return await query.Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }


        //Dashboard
        
        public async Task<int> GetTotalProductsAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<int> GetActiveProductsAsync()
        {
            return await _context.Products.CountAsync(p => p.IsActive);
        }

        public async Task<int> GetOutOfStockProductsAsync()
        {
            return await _context.Products.CountAsync(p => p.Stock == 0);
        }

        public async Task<int> GetLowStockProductsAsync()
        {
            return await _context.Products.CountAsync(p => p.Stock > 0 && p.Stock <= 5);
        }



        //Admin Search and filter...

        public async Task<(List<Product> Items, int TotalCount)> GetAdminPagedAsync(AdminProductQueryDto q)
        {
            IQueryable<Product> query = _context.Products.AsNoTracking();

            //search

            if (!string.IsNullOrWhiteSpace(q.Search))
            {
                var s = q.Search.ToLower();
                query = query.Where(p =>
                p.Name.ToLower().Contains(s) ||
                p.Brand.ToLower().Contains(s) ||
                p.Category.ToLower().Contains(s)
                );
            }

            //Filtering

            if (q.IsActive.HasValue)
                query = query.Where(p => p.IsActive == q.IsActive);

            if (!string.IsNullOrWhiteSpace(q.Category))
                query = query.Where(p => p.Category == q.Category);

            if (!string.IsNullOrWhiteSpace(q.Brand))
                query = query.Where(p => p.Brand == q.Brand);

            //TotalCount
            var totalCount = await query.CountAsync();

            //sorting
            query = (q.SortBy?.ToLower(), q.SortOrder?.ToLower()) switch
            {
                ("price", "asc") => query.OrderBy(p => p.Price),
                ("price","desc") => query.OrderByDescending(p=>p.Price),

                ("stock","asc") => query.OrderBy(p=>p.Stock),
                ("stock","desc") => query.OrderByDescending(p=>p.Stock),

                ("createdat","asc") => query.OrderBy(p=>p.CreatedAt),
                _                   => query.OrderByDescending(p=>p.CreatedAt)
            };

            //Pagination

            var items = await query
                .Skip((q.Page - 1) * q.PageSize)
                .Take(q.PageSize)
                .ToListAsync();

            return (items, totalCount);

        }



    }
}
