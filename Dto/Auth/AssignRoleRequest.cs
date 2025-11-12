namespace EShop.Dto.Auth
{
    public class AssignRoleRequest
    {
        public Guid UserId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
