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

        public async Task<Product?> CreateProductAsync(ProductReqDTO productDTO, string token)
        {
            var isValidRole = await _jwtTokenHelper.ValidateTokenAndRole(token, "Admin");
            if (!isValidRole)
                return null; 

            var product = new Product
            {
                ArabicName = productDTO.ArabicName,
                EnglishName = productDTO.EnglishName,
                Price = productDTO.Price
            };

            await _productRepository.AddProductAsync(product);
            return product;
        }

        public async Task<List<Product>> GetProductsAsync(int page, int pageSize)
        {
            return (await _productRepository.GetAllProductsAsync(page, pageSize)).ToList();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return product?.IsDeleted == true ? null : product; 
        }

        public async Task<Product?> UpdateProductAsync(int id, ProductReqDTO productDTO, string token)
        {
            var isValidRole = await _jwtTokenHelper.ValidateTokenAndRole(token, "Admin");
            if (!isValidRole)
                return null; 

            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null || product.IsDeleted)
                return null; 

            product.ArabicName = productDTO.ArabicName;
            product.EnglishName = productDTO.EnglishName;
            product.Price = productDTO.Price;

            await _productRepository.UpdateProductAsync(product);
            return product;
        }

        public async Task<bool> DeleteProductAsync(int id, string token)
        {
            var isValidRole = await _jwtTokenHelper.ValidateTokenAndRole(token, "Admin");
            if (!isValidRole)
                return false; 

            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null || product.IsDeleted)
                return false;

            await _productRepository.DeleteProductAsync(id);
            return true;
        }
    }
}
