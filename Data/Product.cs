namespace EShop.entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CostPrice { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = new Category();
        public DateTime? ExpiryDate { get; set; }
        public int StockQuantity { get; set; } = 0;
    }
}
