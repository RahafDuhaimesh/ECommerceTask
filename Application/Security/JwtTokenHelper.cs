using ECommerceTask.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceTask.Helpers
{
    public class JwtTokenHelper : IJwtTokenHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _jwtKey = "your-very-long-and-random-secret-key-that-is-at-least-32-bytes-long"; // Same key used for Token generation
        private readonly string _validIssuer = "your_issuer_here";  // Same issuer defined in the Token Generator
        private readonly string _validAudience = "your_audience_here"; // Same audience defined in the Token Generator

        public JwtTokenHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Method to validate the token and return ClaimsPrincipal
        private async Task<ClaimsPrincipal> GetClaimsPrincipalFromTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _validIssuer,
                ValidAudience = _validAudience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                var principal = handler.ValidateToken(token, parameters, out var validatedToken);
                return principal;
            }
            catch (Exception)
            {
                return null; // Invalid token
            }
        }

        // Extract user role from the token
        public async Task<string> GetUserRoleFromTokenAsync()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("Token is missing.");

            var claimsPrincipal = await GetClaimsPrincipalFromTokenAsync(token);

            if (claimsPrincipal == null)
                throw new UnauthorizedAccessException("Invalid token.");

            var userRole = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return userRole ?? "User";  // Default to "User" if no role is found
        }

       

        // Validate token and check if it contains a specific role
        public async Task<bool> ValidateTokenAndRole(string token, string requiredRole)
        {
            var claimsPrincipal = await GetClaimsPrincipalFromTokenAsync(token);

            if (claimsPrincipal == null)
                return false;  // Invalid token

            var userRole = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            return userRole != null && userRole.Equals(requiredRole, StringComparison.OrdinalIgnoreCase);
        }

        // Check if the user has an Admin role
        public async Task<bool> IsUserAdminAsync()
        {
            var role = await GetUserRoleFromTokenAsync();
            return role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<string> GetUserIdFromTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("Token is missing.");

            var claimsPrincipal = await GetClaimsPrincipalFromTokenAsync(token);

            if (claimsPrincipal == null)
                throw new UnauthorizedAccessException("Invalid token.");

            // استخراج UserId من التوكن
            var userId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return userId; // إرجاع الـ UserId
        }



    }
}
