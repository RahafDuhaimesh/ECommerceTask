namespace ECommerceTask.Application.Interfaces
{
    public interface IJwtTokenHelper
    {
        bool ValidateTokenAndRole(string token, string requiredRole);  // Method to validate token and check role
        string? GetUserRoleFromToken(string token);  // Method to extract role from the token
    }
}
