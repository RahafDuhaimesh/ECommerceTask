using ECommerceTask.Application.Interfaces;
using ECommerceTask.Domain.Entities;
using ECommerceTask.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceTask.Helpers;

namespace ECommerceTask.Application.Services
{
    public class InvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenHelper _jwtTokenHelper;

        public InvoiceService(IInvoiceRepository invoiceRepository, IProductRepository productRepository, IUserRepository userRepository, IJwtTokenHelper jwtTokenHelper)
        {
            _invoiceRepository = invoiceRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _jwtTokenHelper = jwtTokenHelper;
        }

        public async Task<InvoiceResDTO> AddInvoiceAsync(List<InvoiceDetailDTO> invoiceDetailsDTO)
        {
            if (invoiceDetailsDTO == null || invoiceDetailsDTO.Count == 0)
                throw new ArgumentException("Invoice details cannot be empty.");

            int userId = invoiceDetailsDTO.First().UserId;

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var invoice = new Invoice
            {
                UserId = user.Id,
                Date = DateTime.UtcNow,
                TotalAmount = 0,
                InvoiceDetails = new List<InvoiceDetail>()
            };

            decimal totalAmount = 0;

            foreach (var detail in invoiceDetailsDTO)
            {
                var product = await _productRepository.GetProductByIdAsync(detail.ProductId);
                if (product == null || product.IsDeleted)
                    throw new KeyNotFoundException($"Product with ID {detail.ProductId} not found or is deleted.");

                if (detail.Quantity <= 0)
                    throw new ArgumentException("Quantity must be greater than zero.");

                var price = product.Price * detail.Quantity;
                totalAmount += price;

                var invoiceDetail = new InvoiceDetail
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    Price = price,
                    Invoice = invoice
                };

                invoice.InvoiceDetails.Add(invoiceDetail);
            }

            invoice.TotalAmount = totalAmount;
            await _invoiceRepository.CreateInvoiceAsync(invoice);

            var invoiceSummary = new InvoiceResDTO
            {
                Id = invoice.Id,
                TotalAmount = invoice.TotalAmount
            };

            return invoiceSummary;
        }




        public async Task<Invoice> GetInvoiceByIdAsync(int invoiceId)
        {
            return await _invoiceRepository.GetInvoiceByIdAsync(invoiceId);
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByUserIdAsync(int userId)
        {
            return await _invoiceRepository.GetInvoicesByUserIdAsync(userId);
        }
    }
}
