using EShop.Dto;
using EShop.Dto.CategoryModel;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Service.Interface
{
    public interface ICategoryService
    {
        Task<BaseResponse<bool>> CreateAsync(CreateCategoryDto request);
        Task<List<CategoryDto>> GetCategoriesAsync();       
        Task<BaseResponse<CategoryDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteAsync(Guid id);
        Task<BaseResponse<bool>> UpdateAsync(Guid id, CreateCategoryDto request);
    }
}
