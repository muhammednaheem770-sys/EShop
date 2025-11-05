using EShop.Dto;
using EShop.Dto.ProductModel;
using EShop.entities;
using EShop.Repositories;
using EShop.Repositories.Interface;
using EShop.service.Interface;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Threading;

namespace EShop.service
{
    public class ProductService(IProductRepository productRepository, ILogger<ProductService> logger) : IProductService
    {
        public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var product = await productRepository.GetProductByIdAsync(id, CancellationToken.None);

                if ((product == null))
                {
                    return BaseResponse<bool>.FailResponse("Product not found.");
                }
                var result = await productRepository.DeleteAsync(product, CancellationToken.None);

                if ((!result))
                {
                    return BaseResponse<bool>.FailResponse("Failes to delete product.");
                }
                return BaseResponse<bool>.SuccessResponse(true, "Product deleted succesfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while deleting product.");
                return BaseResponse<bool>.FailResponse($"Error: (ex.Message)");
            }
        }

        public async Task<BaseResponse<IEnumerable<ProductDto>>> GetAllAsync()
        {
            try
            {
                var products = await productRepository.GetProductsAsync(CancellationToken.None);

                if (products == null || !products.Any())
                    return BaseResponse<IEnumerable<ProductDto>>.FailResponse("No product found.");

                var _context = new List<ProductDto>();
                var productDtos = _context.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Description = p.Description,
                    Price = p.SellingPrice,
                    CostPrice = p.CostPrice,
                    StockQuantity = p.StockQuantity,
                    ExpiryDate = p.ExpiryDate,
                    CreatedAt = p.CreatedAt,
                    //Category = p.Category != null ? p.Category.Name : "Uncategorized"
                });
                return BaseResponse<IEnumerable<ProductDto>>.SuccessResponse(productDtos, "Products retrieved succesfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retriwving all products.");
                return BaseResponse<IEnumerable<ProductDto>>.FailResponse("An error occured while retrieving products.");
            }
        }

        public async Task<BaseResponse<ProductDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var product = await productRepository.GetProductByIdAsync(id, CancellationToken.None);

                if ((product == null))
                {
                    return BaseResponse<ProductDto>.FailResponse("Product not found");
                }
                var productDto = new ProductDto
                {
                    Id = product.Id,
                    ProductName = product.Name,
                    Price = product.SellingPrice,
                    CostPrice = product.CostPrice,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    ExpiryDate = product.ExpiryDate,
                    CreatedAt = product.CreatedAt,
                    StockQuantity = product.StockQuantity,
                };
                return BaseResponse<ProductDto>.SuccessResponse(productDto, "Product retrieved succesfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while fetching product by 10");
                return BaseResponse<ProductDto>.FailResponse($"Error: {ex.Message}");
            }
        }



        public async Task<BaseResponse<IEnumerable<ProductDto>>> GetProductsByCategoryIdAsync(Guid categoryId)
        {
            try
            {
                var products = await productRepository.GetProductsAsync(CancellationToken.None);
                if (products == null || !products.Any())
                    return BaseResponse<IEnumerable<ProductDto>>.FailResponse("No products found.");

                var filteredProducts = products
                    .Where(p => p.Id == categoryId)
                    .ToList();
                if (!filteredProducts.Any())
                    return BaseResponse<IEnumerable<ProductDto>>.FailResponse("No product found for the specified category.");

                var productDtos = filteredProducts.Select(p => new ProductDto
                {
                    Id = p.Id,
                    ProductName = p.Name,
                    Description = p.Description,
                }).ToList();

                return BaseResponse<IEnumerable<ProductDto>>.SuccessResponse(productDtos, "Product retrieved succesfully.");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while fetching products.");
                return BaseResponse<IEnumerable<ProductDto>>.FailResponse($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> UpdateAsyncProduct(Guid id, CreateProductDto request)
        {
            try
            {
                var product = await productRepository.GetProductByIdAsync(id, CancellationToken.None);

                if (product == null)
                    return BaseResponse<bool>.FailResponse("Product not found.");

                product.Name = request.Name;
                product.Description = request.Description;
                product.SellingPrice = request.SellingPrice;
                product.CostPrice = request.CostPrice;
                product.StockQuantity = request.StockQuantity;
                product.CategoryId = request.CategoryId;
                if (request.ExpiryDate.HasValue)
                    product.ExpiryDate = request.ExpiryDate.Value;
                product.UpdatedAt = DateTime.UtcNow;

                var result = await productRepository.UpdateAsync(product, CancellationToken.None);

                if (!result)
                    return BaseResponse<bool>.FailResponse("Failed to update product.");
                return BaseResponse<bool>.SuccessResponse(true, "Product updated succesfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while updating product.");
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}"); ;
            }
        }


        public async Task<BaseResponse<bool>> CreateAsync(CreateProductDto request)
        {
            var cancellationToken = CancellationToken.None;
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
                    Description = request.Description,
                    SellingPrice = request.SellingPrice,
                    CostPrice = request.CostPrice,
                    StockQuantity = request.StockQuantity,
                    CategoryId = request.CategoryId,
                    CreatedAt = DateTime.Now,
                };

                await productRepository.AddAsync(product, cancellationToken);
                return BaseResponse<bool>.SuccessResponse(true, "Product created succesfully.");

            }
            catch (Exception ex)
            {
                logger.LogCritical($"Error: {ex.InnerException?.Message ?? ex.Message}");
                return BaseResponse<bool>.FailResponse("Create your support service.");
            }
        }

        public async Task<BaseResponse<bool>> UpdateAsync(Guid id, CreateProductDto request)
        {
            var cancellationToken = CancellationToken.None;
            try
            {
                var product = await productRepository.GetProductByIdAsync(id, CancellationToken.None);

                if (product == null)
                    return BaseResponse<bool>.FailResponse("Product not found.");

                product.Name = request.Name;
                product.Description = request.Description;
                product.SellingPrice = request.SellingPrice;
                product.CostPrice = request.CostPrice;
                product.StockQuantity = request.StockQuantity;
                product.CategoryId = request.CategoryId;
                if (request.ExpiryDate.HasValue)
                    product.ExpiryDate = request.ExpiryDate.Value;
                product.UpdatedAt = DateTime.UtcNow;

                var result = await productRepository.UpdateAsync(product, CancellationToken.None);

                if (!result)
                    return BaseResponse<bool>.FailResponse("Failed to update product.");
                return BaseResponse<bool>.SuccessResponse(true, "Product updated succesfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while updating product.");
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}"); ;
            }
        }
    }
}



