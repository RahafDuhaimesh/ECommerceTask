using ECommerceTask.Application.DTOs;
using ECommerceTask.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerceTask.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductReqDTO productDTO, [FromHeader] string token)
        {
            var result = await _productService.CreateProductAsync(productDTO, token);
            if (result == null)
                return Unauthorized(new { message = "Admin permissions required." });

            return CreatedAtAction(nameof(GetProduct), new { id = result.Id }, result);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _productService.GetProductsAsync(page, pageSize);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found." });

            return Ok(product);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductReqDTO productDTO, [FromHeader] string token)
        {
            var result = await _productService.UpdateProductAsync(id, productDTO, token);
            if (result == null)
                return Unauthorized(new { message = "Admin permissions required or product not found." });

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id, [FromHeader] string token)
        {
            var result = await _productService.DeleteProductAsync(id, token);
            if (!result)
                return NotFound(new { message = "Product not found or already deleted." });

            return NoContent();
        }
    }
}
