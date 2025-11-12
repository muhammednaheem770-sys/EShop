using EShop.Data;
using EShop.Dto;
using EShop.Dto.Auth;

namespace EShop.service.Interface
{
    public interface IAuthService
    {
        Task<BaseResponse<TokenResponseDto>> LoginAsync(Dto.Auth.LoginRequestDto request, CancellationToken cancellationToken);
        Task<BaseResponse<User>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken);
        Task<BaseResponse<TokenResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken);
    }
}
