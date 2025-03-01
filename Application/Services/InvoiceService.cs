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

        public async Task<Invoice> AddInvoiceAsync(List<InvoiceDetailDTO> invoiceDetailsDTO, string token)
        {
            // استخراج UserId من التوكن باستخدام JwtTokenHelper
            var userIdString = await _jwtTokenHelper.GetUserIdFromTokenAsync(token);  // تمرير التوكن هنا لاستخراج الـ UserId

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
                throw new Exception("User not found or token is invalid.");

            // منطق إضافة الفاتورة
            var user = await _userRepository.GetUserByIdAsync(userId);  // استخدام UserId من نوع int
            if (user == null)
                throw new Exception("User not found.");

            var invoice = new Invoice
            {
                UserId = user.Id,  // استخدام الـ UserId
                Date = DateTime.UtcNow,
                TotalAmount = 0
            };

            decimal totalAmount = 0;

            foreach (var detail in invoiceDetailsDTO)
            {
                var product = await _productRepository.GetProductByIdAsync(detail.ProductId);
                if (product == null || product.IsDeleted)
                    throw new Exception($"Product with ID {detail.ProductId} not found or is deleted.");

                if (detail.Quantity <= 0)
                    throw new Exception("Quantity must be greater than zero.");

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

            return invoice;
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
