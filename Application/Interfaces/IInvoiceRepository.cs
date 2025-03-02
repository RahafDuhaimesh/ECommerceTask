using ECommerceTask.Application.DTOs;
using ECommerceTask.Domain.Entities;

public interface IInvoiceRepository
{
    Task CreateInvoiceAsync(Invoice invoice);
    Task<Invoice> GetInvoiceByIdAsync(int id);
    Task<IEnumerable<Invoice>> GetInvoicesByUserIdAsync(int userId);
}
