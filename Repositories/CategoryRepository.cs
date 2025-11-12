using EShop.Context;
using EShop.entities;
using EShop.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Category category, CancellationToken cancellationToken)
        {
            await _context.Categories.AddAsync(category, cancellationToken);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> CreateAsync(Category category, CancellationToken cancellationToken)
        {
            await _context.AddAsync(category, cancellationToken);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> DeleteAsync(Category category, CancellationToken cancellationToken)
        {
            _context.Remove(category);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Categories.AsNoTracking()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Category?> GetProductByIdAsync(Guid Id, Category category, CancellationToken cancellationToken)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == Id, cancellationToken);
        }

        public async Task<bool> UpdateAsync(Category category, CancellationToken cancellationToken)
        {
            _context.Categories.Update(category);
            return await _context.SaveChangesAsync(cancellationToken) > 0 ? true : false;
        }
    }
}
