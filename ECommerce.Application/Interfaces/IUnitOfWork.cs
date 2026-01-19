

namespace ECommerce.Application.Interfaces
{
    public interface IUnitOfWork
    {
        
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveChangesAsync();
    }
}

