namespace EShop.entities
{
    public class Product : BaseEntity
    {
        public string name { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CostPrice { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public Category category { get; set; }
        public object Category { get; internal set; }
        public DateTime? ExpiryDate { get; set; }
        public int StockQuantity { get; set; }
    }
}
