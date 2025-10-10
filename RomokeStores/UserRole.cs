using EShop.entities;
using EShop.RomokeStores;

namespace Romoke_Stores
{
    public class UserRole :BaseEntity
    {
        public int RoleId { get; set; }
        public string UserId { get; set; }
        public User user { get; set; }
        public Role role { get; set; }
    }
}



