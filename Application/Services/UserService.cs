using ECommerceTask.Application.DTOs;
using ECommerceTask.Application.Interfaces;
using ECommerceTask.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceTask.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
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

    }
}
