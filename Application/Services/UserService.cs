using ECommerceTask.Application.DTOs;
using ECommerceTask.Application.Interfaces;
using ECommerceTask.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceTask.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtTokenHelper _jwtTokenHelper;

        public UserService(IUserRepository userRepository,
                           IPasswordHasher passwordHasher,
                           ITokenGenerator tokenGenerator,
                           IHttpContextAccessor httpContextAccessor,
                           IJwtTokenHelper jwtTokenHelper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _httpContextAccessor = httpContextAccessor;
            _jwtTokenHelper = jwtTokenHelper;
        }

        public async Task<string> RegisterUserAsync(RegisterReqDTO model)
        {
            if (await _userRepository.UserExistsAsync(model.Username))
                return "User already exists.";

            _passwordHasher.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User
            {
                FullName = model.FullName,
                Username = model.Username,
                Email = model.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "User"
            };

            await _userRepository.AddUserAsync(newUser);
            return "User registered successfully.";
        }

        public async Task<LoginResDTO?> LoginUserAsync(LoginReqDTO model)
        {
            var user = await _userRepository.GetUserByUsernameAsync(model.Username);

            if (user == null || !_passwordHasher.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new LoginResDTO
                {
                    Token = string.Empty,
                    Message = "Invalid username or password."
                };
            }

            var token = _tokenGenerator.GenerateToken(user.Username, new List<string> { user.Role });

            return new LoginResDTO { Token = token, Message = "Login successful" };
        }

        public async Task<string> RegisterAdminAsync(RegisterAdminReqDTO model)
        {
            _passwordHasher.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User
            {
                FullName = model.FullName,
                Username = model.Username,
                Email = model.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "Admin"
            };

            await _userRepository.AddUserAsync(newUser);
            return "Admin registered successfully.";
        }

        public async Task<string> UpdateAdminAsync(int adminId, RegisterAdminReqDTO model, string token)
        {
            var isValidRole = await _jwtTokenHelper.ValidateTokenAndRole(token, "Admin");

            if (!isValidRole)
                return "You do not have permission to update an admin.";

            var admin = await _userRepository.GetUserByIdAsync(adminId);
            if (admin == null)
                return "Admin not found.";

            admin.FullName = model.FullName;
            admin.Username = model.Username;
            admin.Email = model.Email;

            if (!string.IsNullOrEmpty(model.Password))
            {
                _passwordHasher.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);
                admin.PasswordHash = passwordHash;
                admin.PasswordSalt = passwordSalt;
            }

            await _userRepository.UpdateUserAsync(admin);
            return "Admin updated successfully.";
        }

        public async Task<string> DeleteAdminAsync(int adminId, string token)
        {
            var isValidRole = await _jwtTokenHelper.ValidateTokenAndRole(token, "Admin");

            if (!isValidRole)
                return "You do not have permission to delete an admin.";

            var admin = await _userRepository.GetUserByIdAsync(adminId);
            if (admin == null)
                return "Admin not found.";

            await _userRepository.DeleteUserAsync(adminId);
            return "Admin deleted successfully.";
        }
    }
}
