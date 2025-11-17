using EShop.Context;
using EShop.Data;
using EShop.Dto.Auth;
using EShop.service;
using EShop.service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
namespace EShop.Dto.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService, ApplicationDbContext dbContext) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ApplicationDbContext _dbContext = dbContext;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto? request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                Log.Warning("Registration failed: request body was null");
                return BadRequest(new { message = "Request body cannot be null." });
            }

            Log.Information("Received registration request for {Email}", request.Email);

            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state for registration of {Email}", request.Email);
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.RegisterAsync(request, cancellationToken);

                if (!response.Success)
                {
                    Log.Warning("Registration failed for {Email}: {Message}", request.Email, response.Message);
                    return BadRequest(response);
                }

                Log.Information("Registration successful for {Email}", request.Email);
                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                Log.Warning("Registration cancelled for {Email}", request.Email);
                return StatusCode(StatusCodes.Status499ClientClosedRequest, new { message = "Request was cancelled." });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected error during registration for {Email}", request.Email);

                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "An error occurred during registration"
                );
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
        {
            if (loginRequestDto == null)
            {
                Log.Warning("Login request was null");
                return BadRequest(new { message = "Request body cannot be null." });
            }

            Log.Information("Login attempt for {Email}", loginRequestDto.Email);

            if (!ModelState.IsValid)
            {
                Log.Warning("Login failed for {Email}: invalid model state", loginRequestDto.Email);
                return BadRequest(ModelState);
            }

            try
            {
                // Validate user credentials via your service
                var response = await _authService.LoginAsync(loginRequestDto, cancellationToken);

                if (!response.Success)
                {
                    Log.Warning("Login failed for {Email}: {Message}", loginRequestDto.Email, response.Message);
                    return Unauthorized(response);
                }

                Log.Information("Login successful for {Email}", loginRequestDto.Email);
                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                Log.Warning("Login cancelled for {Email}", loginRequestDto.Email);
                return StatusCode(StatusCodes.Status499ClientClosedRequest, new { message = "Request was cancelled." });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected error during login for {Email}", loginRequestDto.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during login." });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                Log.Warning("Refresh token request was null");
                return BadRequest(new { message = "Request body cannot be null." });
            }

            if (!ModelState.IsValid)
            {
                Log.Warning("Refresh token request failed validation");
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.RefreshTokenAsync(request, cancellationToken);

                if (!response.Success)
                {
                    Log.Warning("Refresh token failed: {Message}", response.Message);
                    return BadRequest(response);
                }

                Log.Information("Refresh token successful for user");
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected error during refresh token");
                return StatusCode(500, new { message = "An error occurred while refreshing token" });
            }
        }

        [HttpGet("GetAdminData")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminData(CancellationToken cancellationToken)
        {
            try
            {
                var adminData = await (from u in _dbContext.Users
                                       join ur in _dbContext.UserRoles on u.Id equals ur.UserId
                                       join r in _dbContext.Roles on ur.RoleId equals r.Id
                                       where r.Name == "Admin"
                                       select new
                                       {
                                           u.Id,
                                           u.Name,
                                           u.Email
                                       }).ToListAsync(cancellationToken);

                return Ok(new
                {
                    Success = true,
                    Data = adminData
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching admin data");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while retrieving admin data."
                });
            }
        }

    }
}
