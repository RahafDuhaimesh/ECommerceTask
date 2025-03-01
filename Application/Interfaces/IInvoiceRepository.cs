using ECommerceTask.Domain.Entities;

public interface IInvoiceRepository
{
    Task<Invoice> CreateInvoiceAsync(Invoice invoice);
    Task<Invoice> GetInvoiceByIdAsync(int id);
    Task<IEnumerable<Invoice>> GetInvoicesByUserIdAsync(int userId);
}
