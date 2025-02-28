namespace ECommerceTask.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public required string ArabicName { get; set; }
        public required string EnglishName { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    }
}
