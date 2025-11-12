using EShop.Context;
using EShop.Data;
using EShop.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repositories
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (user == null)
                return false;

            context.Users.Remove(user);
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Users.ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await context.Users
                .FirstOrDefaultAsync(u => EF.Functions.Like(u.Email, email), cancellationToken);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<bool> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            context.Users.Update(user);
            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
