//using ECommerceTask.Application.Interfaces;
//using ECommerceTask.Domain.Entities;
//using Microsoft.EntityFrameworkCore;
//using System;

//namespace ECommerceTask.Infrastructure.Repositories
//{
//    public class UserRepository : IUserRepository
//    {
//        private readonly ApplicationDbContext _context;

//        public UserRepository(ApplicationDbContext context)
//        {
//            _context = context;
//        }
//        public async Task<User?> GetByUsernameAsync(string username)
//        {
//            return await _context.Users
//                .AsNoTracking()
//                .FirstOrDefaultAsync(u => u.Username == username);
//        }

//        public async Task<User?> GetByEmailAsync(string email)
//        {
//            return await _context.Users
//                .AsNoTracking()
//                .FirstOrDefaultAsync(u => u.Email == email);
//        }

//        public async Task<bool> UserExistsAsync(string username, string email)
//        {
//            return await _context.Users
//                .AsNoTracking()
//                .AnyAsync(u => u.Username == username || u.Email == email);
//        }

//        public async Task CreateUserAsync(User user)
//        {
//            await _context.Users.AddAsync(user);
//        }
//    }
//}
