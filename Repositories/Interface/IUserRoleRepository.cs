using EShop.Data;

namespace EShop.Repositories.Interface
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetUsersByRoleIdAsync(Guid roleId, CancellationToken cancellationToken);
        Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);
    }
}
