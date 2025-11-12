using EShop.Data;
using EShop.Dto;
using EShop.Dto.RoleModel;

namespace EShop.service.Interface
{
    public interface IRoleService
    {
        Task<BaseResponse<Role>> CreateAsync(CreateRoleDto request , CancellationToken cancellationToken);
        Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken);
        Task<BaseResponse<Role?>> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Guid id, string name, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
