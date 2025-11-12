using EShop.Context;
using EShop.Data;
using EShop.Dto;
using EShop.Repositories.Interface;
using EShop.service.Interface;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EShop.service
{
    public class UserRoleService(ApplicationDbContext dbContext, IUserRoleRepository userRoleRepository) : IUserRoleService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IUserRoleRepository _userRoleRepository;

        public async Task<BaseResponse<string>> AssignRoleAsync(Guid userId, string roleName, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _dbContext.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

                if (user == null)
                    return BaseResponse<string>.FailureResponse("User not found");

                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);
                if (role == null)
                    return BaseResponse<string>.FailureResponse("Role not found");

                if (user.UserRoles.Any(ur => ur.RoleId == role.Id))
                    return BaseResponse<string>.FailureResponse("User already has this role");

                user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
                await _dbContext.SaveChangesAsync(cancellationToken);

                Log.Information("Role {Role} assigned successfully to user {UserId}", roleName, userId);
                return BaseResponse<string>.SuccessResponse("Role assigned successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error assigning role {Role} to user {UserId}", roleName, userId);
                return BaseResponse<string>.FailureResponse("An error occurred while assigning role");
            }
        }

        public async Task<BaseResponse<bool>> AssignRoleToUserAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Assigning role {RoleId} to user {UserId}", roleId, userId);

                var result = await userRoleRepository.AssignRoleToUserAsync(userId, roleId, cancellationToken);

                if (!result)
                {
                    Log.Warning("Failed to assign role {RoleId} to user {UserId}", roleId, userId);
                    return BaseResponse<bool>.FailureResponse("Failed to assign role to user.");
                }

                Log.Information("Role {RoleId} successfully assigned to user {UserId}", roleId, userId);
                return BaseResponse<bool>.SuccessResponse(true, "Role assigned successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while assigning role {RoleId} to user {UserId}", roleId, userId);
                return BaseResponse<bool>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<IEnumerable<Role>>> GetRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Fetching roles for user with ID {UserId}", userId);

                var roles = await userRoleRepository.GetRolesByUserIdAsync(userId, cancellationToken);

                if (roles == null || !roles.Any())
                {
                    Log.Warning("No roles found for user with ID {UserId}", userId);
                    return BaseResponse<IEnumerable<Role>>.FailureResponse("No roles found for this user.");
                }

                Log.Information("Retrieved {Count} roles for user with ID {UserId}", roles.Count(), userId);
                return BaseResponse<IEnumerable<Role>>.SuccessResponse(roles, "Roles retrieved successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while fetching roles for user {UserId}", userId);
                return BaseResponse<IEnumerable<Role>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<IEnumerable<User>>> GetUsersByRoleIdAsync(Guid roleId, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Fetching users for role with ID {RoleId}", roleId);

                var users = await userRoleRepository.GetUsersByRoleIdAsync(roleId, cancellationToken);

                if (users == null || !users.Any())
                {
                    Log.Warning("No users found for role with ID {RoleId}", roleId);
                    return BaseResponse<IEnumerable<User>>.FailureResponse("No users found for this role.");
                }

                Log.Information("Retrieved {Count} users for role with ID {RoleId}", users.Count(), roleId);
                return BaseResponse<IEnumerable<User>>.SuccessResponse(users, "Users retrieved successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while fetching users for role {RoleId}", roleId);
                return BaseResponse<IEnumerable<User>>.FailureResponse($"Error: {ex.Message}");
            }
        }
    }
}
