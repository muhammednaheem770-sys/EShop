using EShop.Context;
using EShop.entities;
using EShop.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repositories
{
    public class ProductRepository(ApplicationDbContext dbContext) : IProductRepository
    {
        public async Task<bool> AddAsync(Product product, CancellationToken cancellationToken)
        {
            await dbContext.AddAsync(product, cancellationToken);
            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }
        public async Task<bool> DeleteAsync(Product product, CancellationToken cancellationToken)
        {
            dbContext.Remove(product);
            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken)
        {
            return await dbContext.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        public async Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            dbContext.Products.Update(product);
            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(Guid CategoryId, CancellationToken cancellationToken)
        {
            return await dbContext.Products
                .Include(p => p.Category)
                .Where (p => p.CategoryId == CategoryId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }
    };
}
