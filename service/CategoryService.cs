using EShop.Dto;
using EShop.Dto.CategoryModel;
using EShop.entities;
using EShop.Repositories.Interface;
using EShop.service.Interface;
using System.Linq.Expressions;

namespace EShop.service
{
    public class CategoryService(ICategoryRepository categoryRepository, ILogger<ICategoryService> logger) : ICategoryService
    {
        public object Categories { get; private set; }

        public async Task<BaseResponse<bool>> CreateAsync(CreateCategoryDto request)
        {
            try
            {
                var category = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description
                };

                var result = await categoryRepository.CreateAsync(category, CancellationToken.None);

                if ((!result))
                {
                    return BaseResponse<bool>.FailResponse("Failed to create category. ");
                }
                return BaseResponse<bool>.SuccessResponse(true, "Category created succesfully. ");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while deleting category.");
                return BaseResponse<bool>.FailResponse($"Error: {ex.Message}");
            }
        }


        public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var category = await categoryRepository.GetByIdAsync(id, CancellationToken.None);

                if ((category == null))
                {
                    return BaseResponse<bool>.FailResponse("Category not found.");
                }
                    var result = await categoryRepository.DeleteAsync(category, CancellationToken.None);

                if ((!result))
                    {
                        return BaseResponse<bool>.FailResponse("Failed to delete category.");
                    }
                    return BaseResponse<bool>.SuccessResponse(true, "Category deleted succesfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while deleting category.");
                return BaseResponse<bool>.FailResponse($"Error; {ex.Message}");
            }
        }
        public async Task<BaseResponse<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            try
            {
                var categories = await categoryRepository.GetAllAsync(CancellationToken.None);

                if (categories == null || !categories.Any())
                    return BaseResponse<IEnumerable<CategoryDto>>.FailResponse("No categories found.");

                var categoryDtos = categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                });

                return BaseResponse<IEnumerable<CategoryDto>>.SuccessResponse(categoryDtos, "categories retrieved succesfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving categories: {ex.Message}");
                return BaseResponse<IEnumerable<CategoryDto>>.FailResponse("An error occured while retrieving categories.");
            }
        }

     
        public async Task<BaseResponse<CategoryDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var category = await categoryRepository.GetByIdAsync(id, CancellationToken.None);

                if (category == null)
                    return BaseResponse<CategoryDto>.FailResponse("Categor not found.");

                var categoryDto = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };
                return BaseResponse<CategoryDto>.SuccessResponse(categoryDto, "Category retrieved succesfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving category by ID: {ex.Message}");
                return BaseResponse<CategoryDto>.FailResponse("An error occured while retrieving the category.");
            }
        }

        public async Task<BaseResponse<bool>> UpdateAsync(Guid id, CreateCategoryDto request)
        {
            try
            {
                var category = await categoryRepository.GetByIdAsync(id, CancellationToken.None);

                if ((category == null))
                {
                    return BaseResponse<bool>.FailResponse("Failed to update category.");
                }
                return BaseResponse<bool>.SuccessResponse(true, "Category updates successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occured while uodating category.");
                return BaseResponse<bool>.FailResponse($"Error: {ex:message}");
            }
        }
    }
}
