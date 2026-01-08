using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace ECommerce.Infrastructure.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
            => await _context.Users.FirstOrDefaultAsync(x => x.Email == email);


        public async Task<bool> EmailExistsAsync(string email)
            => await _context.Users.AnyAsync(x => x.Email == email);

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
