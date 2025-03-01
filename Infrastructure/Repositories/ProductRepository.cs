using ECommerceTask.Application.Interfaces;
using ECommerceTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceTask.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all products with pagination
        public async Task<IEnumerable<Product>> GetAllProductsAsync(int page, int pageSize)
        {
            return await _context.Products
                .Where(p => !p.IsDeleted) // Exclude deleted products
                .Skip((page - 1) * pageSize) // Pagination
                .Take(pageSize) // Pagination
                .ToListAsync();
        }

        // Get product by Id
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted); // Exclude deleted products
        }

        // Add a new product
        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        // Update an existing product
        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // Delete a product (soft delete)
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                product.IsDeleted = true; // Soft delete
                await _context.SaveChangesAsync();
            }
        }
    }
}
