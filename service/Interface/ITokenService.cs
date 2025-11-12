using EShop.Data;
using EShop.entities;
namespace EShop.service.Interface
{
    public interface ITokenService
    {
        string GenerateToken(User user, IEnumerable<string>roles);
        string GenerateRefreshToken();
    }
}
