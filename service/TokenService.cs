using EShop.Configurations;
using EShop.Data;
using EShop.service.Interface;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EShop.service
{
    public class TokenService(JwtSettings jwtSettings) : ITokenService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings;
        public string GenerateRefreshToken()
        {
            Log.Information("Generating refresh token");
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public string GenerateToken(User user, IEnumerable<string> roles)
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.Secret))
            {
                throw new InvalidDataException("JWT key is not configured");
            }
            var secretKey = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256
                );

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.Name)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var expiry = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiry,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
