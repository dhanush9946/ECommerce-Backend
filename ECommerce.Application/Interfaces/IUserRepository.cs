using ECommerce.Domain.Entities;
using System;


namespace ECommerce.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task AddAsync(User user);


        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task UpdateAsync(User user);

        //Dashboard
        Task<int> GetTotalUsersAsync();
        Task<int> GetActiveUsersAsync();
        Task<int> GetBlockedUsersAsync();
        Task<int> GetNewUsersTodayAsync();

    }
}
