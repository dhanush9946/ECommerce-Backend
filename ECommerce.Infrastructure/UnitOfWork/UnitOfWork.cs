

using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.Infrastructure.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            await _transaction!.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction!.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
