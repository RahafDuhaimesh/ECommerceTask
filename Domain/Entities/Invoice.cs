namespace ECommerceTask.Domain.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual List<InvoiceDetail> InvoiceDetails { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }
}
