using Azure.Core;
using EShop.Context;
using EShop.Dto;
using EShop.Dto.ProductModel;
using EShop.entities;
using EShop.Repositories;
using EShop.Repositories.Interface;
using EShop.service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Linq.Expressions;
using System.Threading;

namespace EShop.service
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        private readonly ApplicationDbContext _dbContext;
        public async Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Attempting to delete product with ID {ProductId}", id);

                var product = await productRepository.GetProductByIdAsync(id, CancellationToken.None);

                if ((product == null))
                {
                    Log.Warning("Product with ID {ProductId} not found.", id);
                    return new BaseResponse<bool> (false, "Product not found.");
                }
                var result = await productRepository.DeleteAsync(product, CancellationToken.None);

                if ((!result))
                {
                    Log.Error("Failed to delete product with ID {ProductId}", id);
                    return new BaseResponse<bool>(false, "Failed to delete product.");
                }

                Log.Information("Product with ID {ProductId} deleted successfully.", id);
                return BaseResponse<bool>.SuccessResponse(true, "Product deleted successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting product.");
                return new BaseResponse<bool>(false, $"Error: (ex.Message)");
            }
        }

        public async Task<BaseResponse<IEnumerable<ProductDto>>> GetAllAsync()
        {
            try
            {
                Log.Information("Retrieving all products.");

                var products = await productRepository.GetProductsAsync(CancellationToken.None);

                if (products == null || !products.Any())
                {
                    Log.Warning("No products found in the database.");
                    return new BaseResponse<IEnumerable<ProductDto>>(false, "No product found.");
                }
              
                var productDto = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Description = p.Description,
                    Price = p.SellingPrice,
                    CostPrice = p.CostPrice,
                    StockQuantity = p.StockQuantity,
                    ExpiryDate = p.ExpiryDate,
                    CreatedAt = p.CreatedAt,
                    CategoryName = p.Category != null ? p.Category.Name : "Uncategorized",
                    ProductName = p.Name,
                    CategoryId = p.CategoryId,
                    SellingPrice = p.SellingPrice,
                });

                Log.Information("Retrieved {Count} products successfully.", productDto.Count());
                return BaseResponse<IEnumerable<ProductDto>>.SuccessResponse(productDto, "Products retrieved succesfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving all products.");
                return new BaseResponse<IEnumerable<ProductDto>> (false, "An error occurred while retrieving products.");
            }
        }

        public async Task<BaseResponse<ProductDto>> GetByIdAsync(Guid id)
        {
            try
            {
                Log.Information("Retrieving product with ID {ProductId}", id);

                var product = await productRepository.GetProductByIdAsync(id, CancellationToken.None);
                if (product == null)
                {
                    Log.Warning("Product with ID {ProductId} not found.", id);
                    return new BaseResponse<ProductDto>(false, "Product not found.");
                }

                var productDto = new ProductDto
                {
                    Id = product.Id,
                    ProductName = product.Name,
                    Description = product.Description,
                    SellingPrice = product.SellingPrice,
                    CostPrice = product.CostPrice,
                    StockQuantity = product.StockQuantity,
                    CategoryId = product.CategoryId,
                    ExpiryDate = product.ExpiryDate,
                    CreatedAt = product.CreatedAt
                };

                Log.Information("Product {ProductName} retrieved successfully.", product.Name);
                return BaseResponse<ProductDto>.SuccessResponse(productDto, "Product retrieved successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving product with ID {ProductId}", id);
                return new BaseResponse<ProductDto>(false, $"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<IEnumerable<ProductDto>>> GetProductsByCategoryIdAsync(Guid categoryId)
        {
            try
            {
                Log.Information("Retrieving products for category ID {CategoryId}", categoryId);

                var products = await productRepository.GetProductsAsync(CancellationToken.None);
                if (products == null || !products.Any())
                {
                    Log.Warning("No products found for category ID {CategoryId}", categoryId);
                    return new BaseResponse<IEnumerable<ProductDto>>(false, "No products found.");
                }

                var filtered = products.Where(p => p.CategoryId == categoryId).ToList();
                if (!filtered.Any())
                {
                    Log.Warning("No products found for category ID {CategoryId}", categoryId);
                    return new BaseResponse<IEnumerable<ProductDto>>(false, "No products found for the specified category.");
                }

                var productDto = filtered.Select(p => new ProductDto
                {
                    Id = p.Id,
                    ProductName = p.Name,
                    Description = p.Description,
                    SellingPrice = p.SellingPrice,
                    CategoryId = p.CategoryId
                });

                Log.Information("Retrieved {Count} products for category ID {CategoryId}", productDto.Count(), categoryId);
                return BaseResponse<IEnumerable<ProductDto>>.SuccessResponse(productDto, "Products retrieved successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving products by category ID {CategoryId}", categoryId);
                return new BaseResponse<IEnumerable<ProductDto>>(false, $"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<bool>> CreateAsync(CreateProductDto request, CancellationToken cancellationToken)
        {
           
            try
            {
                if ((request == null))
                {
                    Log.Warning("Attempted to create product with null request data.");
                    return new BaseResponse<bool> (false, "Invalid product data.");
                }
                Log.Information("Creating new product: {ProductName}", request.Name);

                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    SellingPrice = request.SellingPrice,
                    CostPrice = request.CostPrice,
                    StockQuantity = request.StockQuantity,
                    CategoryId = request.CategoryId,
                    ExpiryDate = request.ExpiryDate,
                    
                    CreatedAt = DateTime.Now,
                };

                await productRepository.AddAsync(product, cancellationToken);

                Log.Information("Product {ProductName} created successfully.", request.Name);
                return BaseResponse<bool>.SuccessResponse(true, "Product created successfully.");

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating product {ProductName}", request?.Name);
                return new BaseResponse<bool> (false, "Create your support service.");
            }
        }

        public async Task<BaseResponse<bool>> UpdateAsync(Guid id, CreateProductDto request)
        {
            try
            {
                Log.Information("Updating product with ID {ProductId}", id);

                var product = await productRepository.GetProductByIdAsync(id, CancellationToken.None);
                if (product == null)
                {
                    Log.Warning("Product with ID {ProductId} not found for update.", id);
                    return new BaseResponse<bool>(false, "Product not found.");
                }

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
                {
                    Log.Error("Failed to update product with ID {ProductId}", id);
                    return new BaseResponse<bool>(false, "Failed to update product.");
                }

                Log.Information("Product {ProductName} updated successfully.", product.Name);
                return BaseResponse<bool>.SuccessResponse(true, "Product updated successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating product with ID {ProductId}", id);
                return new BaseResponse<bool>(false, $"Error: {ex.Message}");
            }

        }

        public async Task<BaseResponse<Product>> AddProductAsync(AddProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Attempting to add a new product: {ProductName}", request.Name);

                var existingProduct = await _dbContext.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken);

                if (existingProduct != null)
                {
                    Log.Warning("Product with name {ProductName} already exists", request.Name);
                    return BaseResponse<Product>.FailureResponse("Product already exists.");
                }

                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    CategoryId = request.CategoryId,
                    CreatedAt = DateTime.UtcNow
                };

                await _dbContext.Products.AddAsync(product, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                Log.Information("Product {ProductName} added successfully", request.Name);
                return BaseResponse<Product>.SuccessResponse(product, "Product added successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while adding product {ProductName}", request.Name);
                return BaseResponse<Product>.FailureResponse("An error occurred while adding the product.");
            }
        }
    }
}



