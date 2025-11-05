using EShop.Dto.ProductModel;
using EShop.service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpPost("create-product")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto request)
        {
            if (request == null)
                return BadRequest("Product data cannot be null.");
            var response = await productService.CreateAsync(request);
            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response);
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await productService.GetAllAsync();
            if (!response.Success)
                return NotFound(new { message = response.Message });
            return Ok(response);
        }

        [HttpGet("category/{categoryId:guid}")]
        public async Task<IActionResult> GetProductsByCategoryIdAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                 return BadRequest("Invalid category ID.");

            var response = await productService.GetProductsByCategoryIdAsync(categoryId);
            
            if (!response.Success)
                return NotFound(new {message = response.Message });
            return Ok(response);
        }

    [HttpPut("update-product/{product-id}")]
    public async Task<IActionResult> update([FromRoute(Name = "product id")] Guid id, [FromBody] CreateProductDto request)
    {
        if (request == null)
            return BadRequest("Invalid categoryID.");

        var response = await productService.UpdateAsync(id, request);

        if (!response.Success)
            return BadRequest(response.Message);

        return Ok(response);
    }

        [HttpDelete("delete/{product-id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var response = await productService.DeleteAsync(id, cancellationToken);

            if (!response.Success)
                return NotFound(response.Message);

            return Ok(response);
        }
    }
}
