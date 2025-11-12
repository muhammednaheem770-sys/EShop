using EShop.Data;
using EShop.Dto;
using EShop.Dto.UserModel;
using EShop.Repositories.Interface;
using EShop.service.Interface;
using Serilog;

namespace EShop.service
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<BaseResponse<User>> CreateAsync(CreateUserDto request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Attempting to create user with email: {Email}", request.Email);

                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                {
                    Log.Warning("Invalid user data provided for email: {Email}", request.Email);
                    return BaseResponse<User>.FailureResponse("Invalid user data.");
                }

                var existingUser = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
                if (existingUser != null)
                {
                    Log.Warning("User with email {Email} already exists", request.Email);
                    return BaseResponse<User>.FailureResponse("User already exists.");
                }

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = request.UserName,
                    Email = request.Email,
                    PassWord = request.Password
                };

                await userRepository.CreateAsync(user, cancellationToken);

                Log.Information("User {Email} created successfully", user.Email);
                return BaseResponse<User>.SuccessResponse(user, "User created successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating user {Email}", request.Email);
                return BaseResponse<User>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Attempting to delete user with ID {UserId}", id);
                var user = await userRepository.GetByIdAsync(id, cancellationToken);

                if (user == null)
                {
                    Log.Warning("User with ID {UserId} not found for deletion", id);
                    return BaseResponse<bool>.FailureResponse("User not found.");
                }

                var deleted = await userRepository.DeleteAsync(id, cancellationToken);

                if (!deleted)
                {
                    Log.Error("Failed to delete user with ID {UserId}", id);
                    return BaseResponse<bool>.FailureResponse("Failed to delete user.");
                }

                Log.Information("User with ID {UserId} deleted successfully", id);
                return BaseResponse<bool>.SuccessResponse(true, "User deleted successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting user with ID {UserId}", id);
                return BaseResponse<bool>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<IEnumerable<User>>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Retrieving all users");
                var users = await userRepository.GetAllAsync(cancellationToken);

                if (users == null || !users.Any())
                {
                    Log.Warning("No users found in the database");
                    return BaseResponse<IEnumerable<User>>.FailureResponse("No users found.");
                }

                return BaseResponse<IEnumerable<User>>.SuccessResponse(users, "Users retrieved successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving all users");
                return BaseResponse<IEnumerable<User>>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<User>> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Fetching user with email {Email}", email);
                var user = await userRepository.GetByEmailAsync(email, cancellationToken);

                if (user == null)
                {
                    Log.Warning("User with email {Email} not found", email);
                    return BaseResponse<User?>.FailureResponse("User not found.");
                }

                return BaseResponse<User?>.SuccessResponse(user, "User retrieved successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while fetching user with email {Email}", email);
                return BaseResponse<User?>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Fetching user with ID {UserId}", id);
                var user = await userRepository.GetByIdAsync(id, cancellationToken);

                if (user == null)
                {
                    Log.Warning("User with ID {UserId} not found", id);
                    return BaseResponse<User?>.FailureResponse("User not found.");
                }

                return BaseResponse<User?>.SuccessResponse(user, "User retrieved successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while fetching user with ID {UserId}", id);
                return BaseResponse<User?>.FailureResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> UpdateAsync(Guid id, CreateUserDto request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Attempting to update user with ID {UserId}", id);
                var user = await userRepository.GetByIdAsync(id, cancellationToken);

                if (user == null)
                {
                    Log.Warning("User with ID {UserId} not found for update", id);
                    return BaseResponse<bool>.FailureResponse("User not found.");
                }

                user.UserName = request.UserName;
                user.Email = request.Email;
                user.PassWord = request.Password; 

                var updated = await userRepository.UpdateAsync(user, cancellationToken);

                if (!updated)
                {
                    Log.Error("Failed to update user with ID {UserId}", id);
                    return BaseResponse<bool>.FailureResponse("Failed to update user.");
                }

                Log.Information("User with ID {UserId} updated successfully", id);
                return BaseResponse<bool>.SuccessResponse(true, "User updated successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating user with ID {UserId}", id);
                return BaseResponse<bool>.FailureResponse($"Error: {ex.Message}");
            }
        }
    }
}
