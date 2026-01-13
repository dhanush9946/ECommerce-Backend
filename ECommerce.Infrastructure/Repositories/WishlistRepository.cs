using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly AppDbContext _context;

        public WishlistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Exists(Guid userId, int productId)
        {
            return await _context.Wishlists
                .AnyAsync(w => w.UserId == userId && w.ProductId == productId);
        }

        public async Task Add(Wishlist wishlist)
        {
            await _context.Wishlists.AddAsync(wishlist);
        }

        public async Task<List<Wishlist>> GetByUserId(Guid userId)
        {
            return await _context.Wishlists
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<Wishlist?> Get(Guid userId, int productId)
        {
            return await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);
        }

        public void Remove(Wishlist wishlist)
        {
             _context.Wishlists.Remove(wishlist);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
