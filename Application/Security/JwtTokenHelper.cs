using ECommerceTask.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceTask.Helpers
{
    public class JwtTokenHelper : IJwtTokenHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtTokenHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Validate token and role
        public bool ValidateTokenAndRole(string token, string requiredRole)
        {
            var claimsPrincipal = GetClaimsPrincipalFromToken(token);

            if (claimsPrincipal == null)
                return false; // Invalid token

            var userRole = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            return userRole != null && userRole.Equals(requiredRole, StringComparison.OrdinalIgnoreCase);
        }

        public string? GetUserRoleFromToken(string token)
        {
            var claimsPrincipal = GetClaimsPrincipalFromToken(token);
            return claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        }

        private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("your-very-long-and-random-secret-key-that-is-at-least-32-bytes-long");
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = "your_issuer",
                ValidAudience = "your_audience",
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                var principal = handler.ValidateToken(token, parameters, out var validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
