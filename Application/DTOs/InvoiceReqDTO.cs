namespace ECommerceTask.Application.DTOs
{
    public class InvoiceReqDTO
    {
        public List<InvoiceDetailDTO> Products { get; set; } = new(); // List of products being purchased
    }

   
}
