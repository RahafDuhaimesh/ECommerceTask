namespace ECommerceTask.Application.Interfaces
{
    public interface IJwtTokenHelper
    {
        Task<string> GetUserRoleFromTokenAsync();  // Method to extract role from the token (async)
        Task<bool> IsUserAdminAsync();  // Method to check if the user has an Admin role (async)
        Task<bool> ValidateTokenAndRole(string token, string requiredRole);

        Task<string> GetUserIdFromTokenAsync(string token);

    }
}
