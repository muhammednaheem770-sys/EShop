using EShop.Dto;
using EShop.Dto.ProductModel;
using EShop.entities;

namespace EShop.service.Interface
{
    public interface IProductService
    {
        Task<BaseResponse<bool>> CreateAsync(CreateProductDto request, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> UpdateAsync(Guid id, CreateProductDto request);
        Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<BaseResponse<ProductDto>> GetByIdAsync(Guid id);
        Task<BaseResponse<IEnumerable<ProductDto>>> GetProductsByCategoryIdAsync(Guid categoryId);
        Task<BaseResponse<IEnumerable<ProductDto>>> GetAllAsync();
        Task<BaseResponse<Product>> AddProductAsync(AddProductRequest request, CancellationToken cancellationToken);
    }
}
