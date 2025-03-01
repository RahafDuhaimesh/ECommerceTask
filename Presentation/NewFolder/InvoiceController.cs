using ECommerceTask.Application.Interfaces;
using ECommerceTask.Domain.Entities;
using ECommerceTask.Application.DTOs;
using ECommerceTask.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using ECommerceTask.Helpers;

namespace ECommerceTask.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;
        private readonly IProductRepository _productRepository;
        private readonly IJwtTokenHelper _jwtTokenHelper; // Use _jwtTokenHelper consistently

        public InvoiceController(InvoiceService invoiceService, IProductRepository productRepository, IJwtTokenHelper jwtTokenHelper)
        {
            _invoiceService = invoiceService;
            _productRepository = productRepository;
            _jwtTokenHelper = jwtTokenHelper;  // Make sure this is consistent
        }

        [HttpPost("purchase")]
        [Authorize]
        public async Task<IActionResult> CreateInvoice([FromBody] List<InvoiceDetailDTO> invoiceDetailsDTO, [FromHeader] string token)
        {
            try
            {
                // تحقق من أن التوكن موجود في الهيدر
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { message = "Token is missing." });
                }

                // فك التوكن واستخراج الـ User ID باستخدام JwtTokenHelper
                var userId = await _jwtTokenHelper.GetUserIdFromTokenAsync(token);  // استخدم الدالة لاستخراج الـ UserId

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Invalid token or user not found." });
                }

                // إضافة الفاتورة باستخدام الـ InvoiceService وتمرير التوكن
                var invoice = await _invoiceService.AddInvoiceAsync(invoiceDetailsDTO, token);  // تمرير التوكن مع البيانات

                // إرجاع الفاتورة المُنشأة
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        // Get Invoice by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
                return NotFound();

            return Ok(invoice);
        }

        // Get Invoices by User ID
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetInvoicesByUserId(int userId)
        {
            var invoices = await _invoiceService.GetInvoicesByUserIdAsync(userId);
            return Ok(invoices);
        }
    }
}
