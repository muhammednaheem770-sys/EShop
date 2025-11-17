using BCrypt.Net;
using EShop.Configurations;
using EShop.Context;
using EShop.Data;
using EShop.Dto;
using EShop.Dto.Auth;
using EShop.entities;
using EShop.service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace EShop.service
{
    public class AuthService(ApplicationDbContext dbContext, ITokenService tokenService, JwtSettings _jwtSettings) : IAuthService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<BaseResponse<TokenResponseDto>> LoginAsync(Dto.Auth.LoginRequestDto request, CancellationToken cancellationToken)
        {
            Log.Information("Login attempt for {Email}", request.Email);

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user == null)
            {
                Log.Warning("Login failed: user not found for {Email}", request.Email);
                return BaseResponse<TokenResponseDto>.FailureResponse("Invalid email or password");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                Log.Warning("Login failed: invalid password for {Email}", request.Email);
                return BaseResponse<TokenResponseDto>.FailureResponse("Invalid email or password");
            }

            // ✅ Fetch roles dynamically from UserRoles table
            var roles = await _dbContext.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.Role.Name)
                .ToListAsync(cancellationToken);

            var jwtToken = _tokenService.GenerateToken(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var tokenResponse = new TokenResponseDto
            {
                Token = jwtToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
            };

            Log.Information("Login successful for {Email}", request.Email);
            return BaseResponse<TokenResponseDto>.SuccessResponse(tokenResponse, "Login successful");
        }




        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                    ValidateLifetime = false, // Ignore expired token here
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    Log.Warning("Invalid token algorithm");
                    return null!;
                }

                return principal;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get principal from expired token");
                return null!;
            }
        }

        public async Task<BaseResponse<TokenResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Attempting to refresh token for refresh token: {RefreshToken}", request.RefreshToken);

                // Find the user with the provided refresh token
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken, cancellationToken);

                if (user == null)
                {
                    Log.Warning("Refresh token invalid: no user found with token {RefreshToken}", request.RefreshToken);
                    return BaseResponse<TokenResponseDto>.FailureResponse("Invalid refresh token");
                }

                // Check if refresh token is expired
                if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    Log.Warning("Refresh token expired for user {Email}", user.Email);
                    return BaseResponse<TokenResponseDto>.FailureResponse("Refresh token has expired");
                }

                // Generate new JWT and refresh token
                var newJwtToken = _tokenService.GenerateToken(user, new List<string>()); // Pass roles if needed
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                // Update user's refresh token and expiry in database
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _dbContext.SaveChangesAsync(cancellationToken);

                Log.Information("Refresh token successful for user {Email}", user.Email);

                // Prepare response DTO
                var tokenResponse = new TokenResponseDto
                {
                    Token = newJwtToken,
                    RefreshToken = newRefreshToken,
                    RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
                };

                return BaseResponse<TokenResponseDto>.SuccessResponse(tokenResponse, "Token refreshed successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while refreshing token {RefreshToken}", request.RefreshToken);
                return BaseResponse<TokenResponseDto>.FailureResponse("An error occurred while refreshing token");
            }
        }


        public async Task<BaseResponse<User>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Attempting to register new user: {Email}", request.Email);

                // Check if user already exists
                var existingUser = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

                if (existingUser != null)
                {
                    Log.Warning("Registration failed: User with email {Email} already exists", request.Email);
                    return BaseResponse<User>.FailureResponse("User already exists");
                }

                // Create new user
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Name = $"{request.FirstName} {request.LastName}",
                    LastName = request.LastName,
                    ThirdName = request.ThirdName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    //Gender = (EShop.Data.Gender)request.Gender,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    RefreshToken = _tokenService.GenerateRefreshToken(),
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7),
                    CreatedAt = DateTime.UtcNow
                };

                await _dbContext.Users.AddAsync(newUser, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                Log.Information("User registered successfully: {Email}, Id: {UserId}", newUser.Email, newUser.Id);

                // Map to response DTO (no sensitive data)
                var userDto = new UserResponseDto
                {
                    Id = newUser.Id,
                    Name = newUser.Name,
                    LastName = newUser.LastName,
                    ThirdName = newUser.ThirdName,
                    Email = newUser.Email,
                    PhoneNumber = newUser.PhoneNumber,
                    Address = newUser.Address,
                    Gender = newUser.Gender,
                    CreatedAt = newUser.CreatedAt
                };

                return BaseResponse<User>.SuccessResponse(newUser, "User registered successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred during registration for {Email}", request.Email);
                return BaseResponse<User>.FailureResponse("An error occurred during registration");
            }
        }
    }
}
