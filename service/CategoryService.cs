using EShop.Dto;
using EShop.Dto.CategoryModel;
using EShop.entities;
using EShop.Repositories.Interface;
using EShop.Service.Interface;
using Serilog;

namespace EShop.Service
{
    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
    {
        public async Task<BaseResponse<bool>> CreateAsync(CreateCategoryDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    Log.Warning("Category creation failed: Name is missing.");
                    return new BaseResponse<bool>(false, "Invalid data! Category name is required.");
                }

                Log.Information("Creating new category: {CategoryName}", request.Name);

                var category = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description
                };

                var result = await categoryRepository.CreateAsync(category, CancellationToken.None);

                if ((!result))
                {
                    Log.Error("Failed to create category {CategoryName}", request.Name);
                    return new BaseResponse<bool>(false, "Failed to create category. ");
                }

                Log.Information("Category {CategoryName} created successfully", request.Name);
                return BaseResponse<bool>.SuccessResponse(true, "Category created successfully. ");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting category.");
                return new BaseResponse<bool>(false, $"Error: {ex.Message}");
            }
        }


        public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
        {
            try
            {
                Log.Information("Attempting to delete category with ID {CategoryId}", id);

                var category = await categoryRepository.GetByIdAsync(id, CancellationToken.None);

                if ((category == null))
                {
                    Log.Warning("Category with ID {CategoryId} not found for deletion", id);
                    return new BaseResponse<bool>(false, "Category not found.");
                }
                var deleted = await categoryRepository.DeleteAsync(category, CancellationToken.None);

                if ((!deleted))
                {
                    Log.Error("Failed to delete category with ID {CategoryId}", id);
                    return new BaseResponse<bool>(false, "Failed to delete category.");
                }

                Log.Information("Category with ID {CategoryId} deleted successfully", id);
                return BaseResponse<bool>.SuccessResponse(true, "Category deleted successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting category.");
                return new BaseResponse<bool>(false, $"Error; {ex.Message}");
            }
        }
        public async Task<BaseResponse<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            try
            {
                Log.Information("Retrieving all categories");

                var categories = await categoryRepository.GetAllAsync();
                if (categories == null || !categories.Any())
                {
                    Log.Warning("No categories found in the database");
                    return new BaseResponse<IEnumerable<CategoryDto>>(false, "No categories found.");
                }

                var categoryDto = categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                });

                Log.Information("Retrieved {Count} categories successfully", categoryDto.Count());
                return BaseResponse<IEnumerable<CategoryDto>>.SuccessResponse(categoryDto, "categories retrieved succesfully.");
            }
            catch (Exception ex)
            {
                Log.Error($"Error retrieving categories: {ex.Message}");
                return new BaseResponse<IEnumerable<CategoryDto>>(false, "An error occurred while retrieving categories.");
            }
        }


        public async Task<BaseResponse<CategoryDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Retrieving category with ID {CategoryId}", id);

                var category = await categoryRepository.GetByIdAsync(id, CancellationToken.None);

                if (category == null)
                {
                    Log.Warning("Category with ID {CategoryId} not found", id);
                    return new BaseResponse<CategoryDto>(false, "Category not found.");
                }

                var categoryDto = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };
                Log.Information("Category with ID {CategoryId} retrieved successfully", id);
                return BaseResponse<CategoryDto>.SuccessResponse(categoryDto, "Category retrieved successfully.");
            }
            catch (Exception ex)
            {
                Log.Error($"Error retrieving category by ID: {ex.Message}");
                return new BaseResponse<CategoryDto>(false, "An error occurred while retrieving the category.");
            }
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            try
            {
                Log.Information("Retrieving all categories (lightweight DTO list)");

                var categories = await categoryRepository.GetAllAsync();
                var categoryDto = categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                }).ToList();

                Log.Information("Retrieved {Count} categories successfully", categoryDto.Count);
                return categoryDto;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving category list");
                return new List<CategoryDto>();
            }
        }

        public async Task<BaseResponse<bool>> UpdateAsync(Guid id, CreateCategoryDto request)
        {
            try
            {
                Log.Information("Updating category with ID {CategoryId}", id);

                var category = await categoryRepository.GetByIdAsync(id, CancellationToken.None);

                if (category == null)
                {
                    Log.Warning("Category with ID {CategoryId} not found for update", id);
                    return new BaseResponse<bool>(false, "Failed to update category.");
                }
                category.Name = request.Name;
                category.Description = request.Description;

                var updated = await categoryRepository.UpdateAsync(category, CancellationToken.None);
                if (!updated)
                {
                    Log.Error("Failed to update category with ID {CategoryId}", id);
                    return new BaseResponse<bool>(false, "Failed to update category.");
                }

                Log.Information("Category with ID {CategoryId} updated successfully", id);
                return BaseResponse<bool>.SuccessResponse(true, "Category updated successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating category with ID {CategoryId}", id);
                return new BaseResponse<bool>(false, $"Error: {ex.Message}");
            }
        }
    }
}
