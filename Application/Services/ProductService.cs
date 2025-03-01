using ECommerceTask.Application.DTOs;
using ECommerceTask.Application.Interfaces;
using ECommerceTask.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceTask.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IJwtTokenHelper _jwtTokenHelper;

        public ProductService(IProductRepository productRepository, IJwtTokenHelper jwtTokenHelper)
        {
            _productRepository = productRepository;
            _jwtTokenHelper = jwtTokenHelper;
        }

        // Admin only can create a new product
        public async Task<Product?> CreateProductAsync(ProductReqDTO productDTO, string token)
        {
            // Check if the user has an "Admin" role before allowing product creation
            var isValidRole = await _jwtTokenHelper.ValidateTokenAndRole(token, "Admin");
            if (!isValidRole)
                return null; // User is not authorized to create products

            var product = new Product
            {
                ArabicName = productDTO.ArabicName,
                EnglishName = productDTO.EnglishName,
                Price = productDTO.Price
            };

            await _productRepository.AddProductAsync(product);
            return product;
        }

        // Non-admin users can only list products with pagination
        public async Task<List<Product>> GetProductsAsync(int page, int pageSize)
        {
            return (await _productRepository.GetAllProductsAsync(page, pageSize)).ToList();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return product?.IsDeleted == true ? null : product; // Check if deleted
        }

        // Admin can update products
        public async Task<Product?> UpdateProductAsync(int id, ProductReqDTO productDTO, string token)
        {
            // Check if the user has an "Admin" role before allowing product update
            var isValidRole = await _jwtTokenHelper.ValidateTokenAndRole(token, "Admin");
            if (!isValidRole)
                return null; // User is not authorized to update products

            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null || product.IsDeleted)
                return null; // Product is not found or already deleted

            product.ArabicName = productDTO.ArabicName;
            product.EnglishName = productDTO.EnglishName;
            product.Price = productDTO.Price;

            await _productRepository.UpdateProductAsync(product);
            return product;
        }

        // Admin can delete products (soft delete)
        public async Task<bool> DeleteProductAsync(int id, string token)
        {
            // Check if the user has an "Admin" role before allowing product deletion
            var isValidRole = await _jwtTokenHelper.ValidateTokenAndRole(token, "Admin");
            if (!isValidRole)
                return false; // User is not authorized to delete products

            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null || product.IsDeleted)
                return false; // Product not found or already deleted

            await _productRepository.DeleteProductAsync(id);
            return true;
        }
    }
}
