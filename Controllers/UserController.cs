using EShop.Dto.CategoryModel;
using EShop.Dto.UserModel;
using EShop.service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost("create-user")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Email is required.");

            var response = await userService.CreateAsync(request, cancellationToken);
            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var response = await userService.GetAllAsync(cancellationToken);
            return Ok(response);
        }

        [HttpGet("get-by-id/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var response = await userService.GetByIdAsync(id, cancellationToken);
            if (!response.Success || response.Data == null)
                return NotFound(response.Message);

            return Ok(response);
        }

        [HttpGet("get-by-email")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email cannot be empty.");

            var response = await userService.GetByEmailAsync(email, cancellationToken);
            if (!response.Success || response.Data == null)
                return NotFound(response.Message);

            return Ok(response);
        }

        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateUserDto request, CancellationToken cancellationToken)
        {
            var response = await userService.UpdateAsync(id, request, cancellationToken);
            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var response = await userService.DeleteAsync(id, cancellationToken);
            if (!response.Success)
                return NotFound(response.Message);

            return Ok(response);
        }
    }
}
