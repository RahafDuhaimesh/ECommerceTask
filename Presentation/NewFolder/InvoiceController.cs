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
        private readonly IJwtTokenHelper _jwtTokenHelper; 

        public InvoiceController(InvoiceService invoiceService, IProductRepository productRepository, IJwtTokenHelper jwtTokenHelper)
        {
            _invoiceService = invoiceService;
            _productRepository = productRepository;
            _jwtTokenHelper = jwtTokenHelper;  
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> CreateInvoice([FromBody] List<InvoiceDetailDTO> invoiceDetailsDTO)
        {
            try
            {
                if (invoiceDetailsDTO == null || invoiceDetailsDTO.Count == 0)
                    return BadRequest(new { message = "Invoice details are required." });

                var invoice = await _invoiceService.AddInvoiceAsync(invoiceDetailsDTO);

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
                return NotFound();

            return Ok(invoice);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetInvoicesByUserId(int userId)
        {
            var invoices = await _invoiceService.GetInvoicesByUserIdAsync(userId);
            return Ok(invoices);
        }
    }
}
