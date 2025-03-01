namespace ECommerceTask.Application.DTOs
{
    public class InvoiceResDTO
    {
        public int Id { get; set; } // Invoice ID
        public DateTime Date { get; set; } // Invoice creation date
        public int UserId { get; set; } // ID of the user who created the invoice
        public string UserFullName { get; set; } // User's full name
        public decimal TotalAmount { get; set; } // Total amount for the invoice
        public List<InvoiceDetailResDTO> InvoiceDetails { get; set; } = new(); // List of products in the invoice
    }

   
}
