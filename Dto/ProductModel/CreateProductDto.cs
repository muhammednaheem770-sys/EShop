using System.ComponentModel.DataAnnotations;

namespace EShop.Dto.ProductModel
{
    public class CreateProductDto
    {
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CostPrice { get; set; }
        public int StockQuantity { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }
    }
}
