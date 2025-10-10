using EShop.Dto;
using EShop.Dto.ProductModel;
using EShop.entities;
using EShop.service.Interface;

namespace EShop.service
{
    public class ProductService(IProductRepository productRepository, ILogger<ProductService> logger) : IProductService
    {

        public async Task<BaseResponse<bool>> CreateAsync(CreateProductDto request)
        {
            try
            {
                if ((request == null))
                {
                    return BaseResponse<bool>.FailResponse("Invalid product data.");

                }
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description
                };

                var result = await _productRepository.CreateAsync(product, CancellationToken.None);

                if (result)
                {
                    return BaseResponse<bool>.SuccessResponse(true, "Product created successfully.");
                }
                else
                {
                    return BaseResponse<bool>.FailResponse("Failed to create product.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating product.");
                return BaseResponse<bool>.FailResponse("An unexpected error occurred.");
            }
        }

        public Task<BaseResponse<bool>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<IEnumerable<ProductDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<ProductDto>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<IEnumerable<ProductDto>>> GetProductsByCategoryIdAsync(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> UpdateAsync(Guid id, CreateProductDto request)
        {
            throw new NotImplementedException();
        }
    }

    public class Product : BaseEntity
    {
        public object Name { get; set; }
        public object Description { get; set; }
    }
}
