using EShop.Dto.CategoryModel;
using EShop.Service;
using EShop.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto request)
        {
            try
            {
                if (request == null)
                {
                    Log.Warning("Create category failed: Request body is null");
                    return BadRequest("Request body cannot be null.");
                }

                if (!ModelState.IsValid)
                {
                    Log.Warning("Create category failed: Invalid model state");
                    return BadRequest(ModelState);
                }

                var response = await categoryService.CreateAsync(request);

                if (!response.Success)
                {
                    Log.Warning("Create category failed: {Message}", response.Message);
                    return BadRequest(response);
                }

                Log.Information("Category created successfully: {CategoryName}", request.Name);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred while creating category");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var response = await categoryService.GetByIdAsync(id, cancellationToken);

                if (response == null)
                {
                    Log.Warning("Get category by ID {CategoryId} not found", id);
                    return NotFound($"Category with ID {id} not found");
                }

                Log.Information("Category retrieved: {CategoryId}", id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred while retrieving category {CategoryId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var response = await categoryService.GetCategoriesAsync();
                Log.Information("Retrieved {Count} categories", response?.Count ?? 0);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred while retrieving all categories");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CreateCategoryDto request)
        {
            try
            {
                if (request == null)
                {
                    Log.Warning("Update category failed: Request body is null");
                    return BadRequest("Request body cannot be null.");
                }

                var response = await categoryService.UpdateAsync(id, request);

                if (!response.Success)
                {
                    Log.Warning("Update category failed for {CategoryId}: {Message}", id, response.Message);
                    return BadRequest(response);
                }

                Log.Information("Category updated successfully: {CategoryId}", id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred while updating category {CategoryId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var response = await categoryService.DeleteAsync(id);

                if (!response.Success)
                {
                    Log.Warning("Delete category failed for {CategoryId}: {Message}", id, response.Message);
                    return BadRequest(response);
                }

                Log.Information("Category deleted successfully: {CategoryId}", id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred while deleting category {CategoryId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
