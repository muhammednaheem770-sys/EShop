using EShop.entities;

namespace EShop.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool>CreateAsync(Category category, CancellationToken cancellationToken);
        Task<bool>UpdateAsync(Category category, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Category category, CancellationToken cancellationToken);
    }
}
