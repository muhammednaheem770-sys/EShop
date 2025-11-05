using EShop.entities;

namespace EShop.RomokeStores
{
    public class Role : BaseEntity
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
