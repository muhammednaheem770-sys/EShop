using EShop.Context;
using EShop.Data;
using EShop.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repositories
{
    public class RoleRepository(ApplicationDbContext context) : IRoleRepository
    {
        public async Task<Role> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            await context.Roles.AddAsync(role, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return role;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var role = await context.Roles
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (role == null)
                return false;

            context.Roles.Remove(role);
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Roles.ToListAsync(cancellationToken);
        }

        public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await context.Roles
               .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<bool> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            context.Roles.Update(role);
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await context.Roles
                .FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower(), cancellationToken);
        }
    }
 }
