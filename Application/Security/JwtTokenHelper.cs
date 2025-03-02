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
        private readonly string _jwtKey = "your-very-long-and-random-secret-key-that-is-at-least-32-bytes-long";
        private readonly string _validIssuer = "your_issuer_here";
        private readonly string _validAudience = "your_audience_here";

        public JwtTokenHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

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
            catch
            {
                return null;
            }
        }

        public async Task<string> GetUserIdFromTokenAsync()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("Token is missing.");

            var claimsPrincipal = await GetClaimsPrincipalFromTokenAsync(token);
            if (claimsPrincipal == null)
                throw new UnauthorizedAccessException("Invalid token.");

            var userId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User ID not found in token.");

            return userId;
        }

        public async Task<string> GetUserRoleFromTokenAsync()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("Token is missing.");

            var claimsPrincipal = await GetClaimsPrincipalFromTokenAsync(token);
            if (claimsPrincipal == null)
                throw new UnauthorizedAccessException("Invalid token.");

            var userRole = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            return userRole ?? "User";
        }

        public async Task<bool> ValidateTokenAndRole(string token, string requiredRole)
        {
            var claimsPrincipal = await GetClaimsPrincipalFromTokenAsync(token);
            if (claimsPrincipal == null)
                return false;

            var userRole = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            return userRole != null && userRole.Equals(requiredRole, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<bool> IsUserAdminAsync()
        {
            var role = await GetUserRoleFromTokenAsync();
            return role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }


        //public async Task<int> GetUserIdFromTokenAsync(string token)
        //{
        //    if (string.IsNullOrEmpty(token))
        //        throw new UnauthorizedAccessException("Token is missing.");

        //    var handler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_jwtKey);

        //    var parameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = true,
        //        ValidIssuer = _validIssuer,
        //        ValidAudience = _validAudience,
        //        IssuerSigningKey = new SymmetricSecurityKey(key)
        //    };

        //    try
        //    {
        //        var principal = handler.ValidateToken(token, parameters, out _);
        //        var userIdString = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        //        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        //            throw new UnauthorizedAccessException("User ID is invalid or not found in token.");

        //        return userId;
        //    }
        //    catch
        //    {
        //        throw new UnauthorizedAccessException("Invalid token.");
        //    }
        //}

    }
}
