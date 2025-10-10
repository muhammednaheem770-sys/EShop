using EShop.entities;

namespace EShop.service
{
    public interface IProductRepository
    {
        Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken);
        Task<Product?> GetByIDAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> CreateAsyncProduct(Product product, CancellationToken cancellationToken);
        Task<bool> UpdateAsyncProduct(Product product, CancellationToken cancellationToken);  
        Task<bool> DeleteAsyncProduct(Product product, CancellationToken cancellationToken);
    }
}