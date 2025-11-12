using EShop.entities;

namespace EShop.Dto.ProductModel
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public Guid CategoryId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int StockQuantity { get; set; }
    }
}
