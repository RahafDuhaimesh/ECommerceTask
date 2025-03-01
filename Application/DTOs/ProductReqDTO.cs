namespace ECommerceTask.Application.DTOs
{
    public class ProductReqDTO
    {
        public required string ArabicName { get; set; }
        public required string EnglishName { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
