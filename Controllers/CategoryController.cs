using EShop.Dto.CategoryModel;
using EShop.service;
using EShop.service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace EShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(CategoryService categoryService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto request)
        {
            if (request == null)
                return BadRequest("Request body cannot be null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await categoryService.CreateAsync(request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var response = await categoryService.GetByIdAsync(id);

            if (!response.Success)
                return NotFound(response.Message);

            return Ok(response);
        }

        [HttpPost("{Id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateCategoryDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data submitted");

            var response = await categoryService.UpdateAsync(id, request);
            return response.Success ? Ok(response) : BadRequest(response);

        }

        [HttpDelete("{Id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await categoryService.DeleteAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
