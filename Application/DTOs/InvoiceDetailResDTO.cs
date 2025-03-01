namespace ECommerceTask.Application.DTOs
{
    public class InvoiceDetailResDTO
    {
        public int ProductId { get; set; } // Product ID
        public string ProductName { get; set; } // Product name (you will need to get the product name from the database)
        public decimal Price { get; set; } // Product price
        public int Quantity { get; set; } // Quantity purchased
        public decimal TotalPrice { get { return Price * Quantity; } } // Total price for this product in the invoice
    }
}
