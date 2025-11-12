using EShop.Data;
using EShop.Dto;
using EShop.Dto.RoleModel;
using EShop.Repositories;
using EShop.Repositories.Interface;
using EShop.service.Interface;
using Serilog;

namespace EShop.service
{
    public class RoleService(IRoleRepository roleRepository) : IRoleService
    {
        public async Task<BaseResponse<Role>> CreateAsync(CreateRoleDto request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Checking if role {RoleName} already exists", request.Name);

                var existingRole = await roleRepository.GetByNameAsync(request.Name, cancellationToken);
                if (existingRole != null)
                {
                    Log.Warning("Role {RoleName} already exists", request.Name);
                    return BaseResponse<Role>.FailureResponse("Role already exists");
                }

                var newRole = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description
                };

                await roleRepository.CreateAsync(newRole, cancellationToken);
                Log.Information("Role {RoleName} created successfully", request.Name);

                return BaseResponse<Role>.SuccessResponse(newRole, "Role created successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating role {RoleName}", request.Name);
                return BaseResponse<Role>.FailureResponse("An error occurred while creating the role");
            }
        }

        public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Retrieving role by ID {RoleId}", id);
                var role = await roleRepository.GetByIdAsync(id, cancellationToken);
                return role;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving role by ID {RoleId}", id);
                return null;
            }
        }

        public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Retrieving all roles");
                var roles = await roleRepository.GetAllAsync(cancellationToken);
                return roles;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving all roles");
                return Enumerable.Empty<Role>();
            }
        }

        public async Task<BaseResponse<Role?>> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Retrieving role by name {RoleName}", name);
                var role = await roleRepository.GetByNameAsync(name, cancellationToken);

                if (role == null)
                {
                    Log.Warning("Role {RoleName} not found", name);
                    return BaseResponse<Role?>.FailureResponse("Role not found");
                }

                return BaseResponse<Role?>.SuccessResponse(role, "Role retrieved successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving role by name {RoleName}", name);
                return BaseResponse<Role?>.FailureResponse("An error occurred while retrieving the role");
            }
        }

        public async Task<bool> UpdateAsync(Guid id, string name, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Updating role with ID {RoleId}", id);

                var role = await roleRepository.GetByIdAsync(id, cancellationToken);
                if (role == null)
                {
                    Log.Warning("Role with ID {RoleId} not found for update", id);
                    return false;
                }

                role.Name = name;
                await roleRepository.UpdateAsync(role, cancellationToken);

                Log.Information("Role {RoleId} updated successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating role with ID {RoleId}", id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Deleting role with ID {RoleId}", id);

                var role = await roleRepository.GetByIdAsync(id, cancellationToken);
                if (role == null)
                {
                    Log.Error("Role with ID {RoleId} not found for deletion", id);
                    return false;
                }

                await roleRepository.DeleteAsync(role.Id, cancellationToken);
                Log.Information("Role {RoleId} deleted successfully", id);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting role with ID {RoleId}", id);
                return false;
            }
        }
    }
}

