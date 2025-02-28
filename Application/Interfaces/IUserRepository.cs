using ECommerceTask.Domain.Entities;

namespace ECommerceTask.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmail(string email);
        Task<bool> UserExists(string username, string email);
        Task CreateUser(User user);
    }
}
