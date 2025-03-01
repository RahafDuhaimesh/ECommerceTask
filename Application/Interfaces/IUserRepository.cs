using ECommerceTask.Domain.Entities;
using System.Threading.Tasks;

namespace ECommerceTask.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id); // Add GetUserById for CRUD operations
        Task AddUserAsync(User user);
        Task<bool> UserExistsAsync(string username);
        Task UpdateUserAsync(User user); // Add UpdateUser for updating user details
        Task DeleteUserAsync(int userId); // Add DeleteUser for deleting a user
    }
}
