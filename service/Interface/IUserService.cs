using EShop.Data;
using EShop.Dto;
using EShop.Dto.UserModel;

namespace EShop.service.Interface
{
    public interface IUserService
    {
        Task<BaseResponse<User>> CreateAsync(CreateUserDto request, CancellationToken cancellationToken);
        Task<BaseResponse<User?>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<BaseResponse<User?>> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<BaseResponse<IEnumerable<User>>> GetAllAsync(CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> UpdateAsync(Guid id, CreateUserDto request, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> PromoteToAdminAsync(Guid userId);
    }
}
