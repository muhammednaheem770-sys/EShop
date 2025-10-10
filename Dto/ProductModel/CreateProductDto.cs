namespace EShop.Dto.ProductModel
{
    public class CreateProductDto
    {
        public string name { get; set; }
        public object Name { get; internal set; }
        public string description { get; set; }
        public object Description { get; internal set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CostPrice { get; set; }
        public int StockQuantity { get; set; }
        public Guid CategoryId { get; set; }
    }
}
