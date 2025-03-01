using ECommerceTask.Application.Interfaces;
using ECommerceTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECommerceTask.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a new invoice
        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        // Get invoice by ID
        public async Task<Invoice> GetInvoiceByIdAsync(int id)
        {
            return await _context.Invoices
                                 .Include(i => i.InvoiceDetails)
                                 .ThenInclude(id => id.Product)
                                 .FirstOrDefaultAsync(i => i.Id == id);
        }

        // Get all invoices for a user by their user ID
        public async Task<IEnumerable<Invoice>> GetInvoicesByUserIdAsync(int userId)
        {
            return await _context.Invoices
                                 .Where(i => i.UserId == userId)
                                 .Include(i => i.InvoiceDetails)
                                 .ThenInclude(id => id.Product)
                                 .ToListAsync();
        }
    }

}
