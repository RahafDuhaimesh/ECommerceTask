using ECommerceTask.Domain.Entities;
using System.Threading.Tasks;

namespace ECommerceTask.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task<bool> UserExistsAsync(string username);
        Task UpdateUserAsync(User user); 
        Task DeleteUserAsync(int userId); 
    }
}
