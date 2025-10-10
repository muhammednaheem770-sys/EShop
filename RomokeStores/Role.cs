using EShop.entities;

namespace EShop.RomokeStores
{
    public class Role : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
