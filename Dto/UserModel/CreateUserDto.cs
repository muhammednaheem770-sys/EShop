using EShop.Data;

namespace EShop.Dto.UserModel
{
    public class CreateUserDto
    {
        public string Name { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? ThirdName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = default!;
        public string Address { get; set; } = default!;
        public Gender Gender { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
