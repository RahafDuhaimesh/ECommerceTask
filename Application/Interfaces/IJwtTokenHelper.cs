namespace ECommerceTask.Application.Interfaces
{
    public interface IJwtTokenHelper
    {
        Task<string> GetUserRoleFromTokenAsync();
        Task<bool> IsUserAdminAsync();
        Task<bool> ValidateTokenAndRole(string token, string requiredRole);
    }
} 
