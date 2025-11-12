using EShop.Data;
using EShop.Dto;

namespace EShop.service.Interface
{
    public interface IUserRoleService
    {
        Task<BaseResponse<IEnumerable<Role>>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<BaseResponse<IEnumerable<User>>> GetUsersByRoleIdAsync(Guid roleId, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> AssignRoleToUserAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);
        Task<BaseResponse<string>> AssignRoleAsync(Guid userId, string roleName, CancellationToken cancellationToken);

    }
}
