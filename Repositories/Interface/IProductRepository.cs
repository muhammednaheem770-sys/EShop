using EShop.entities;

namespace EShop.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken);
        Task<Product> GetProductAsync(Guid id,  CancellationToken cancellationToken);
        Task<bool> AddASync(Product product, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Product product, CancellationToken cancellationToken);
    }
}
