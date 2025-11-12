using EShop.Data;

namespace EShop.Repositories.Interface
{
    public interface IRoleRepository
    {
        Task<Role> CreateAsync(Role role, CancellationToken cancellationToken);
        Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Role role, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
