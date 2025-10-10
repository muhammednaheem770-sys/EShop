namespace EShop.entities
{
    public class Product : BaseEntity
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int CostPrice { get; set; }
        public int SellingPrice { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
