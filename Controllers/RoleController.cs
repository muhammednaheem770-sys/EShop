using EShop.Dto.RoleModel;
using EShop.service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Controllers
{
    [Route("api/[controller]")]
    public class RoleController(IRoleService roleService) : ControllerBase
    {
        [HttpPost("create-role")]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Role name is required.");

            var response = await roleService.CreateAsync(request, cancellationToken);
            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var response = await roleService.GetAllAsync(cancellationToken);
            return Ok(response);
        }

        [HttpGet("get-by-name")]
        public async Task<IActionResult> GetByName([FromQuery] string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Role name is required.");

            var response = await roleService.GetByNameAsync(name, cancellationToken);
            if (!response.Success || response.Data == null)
                return NotFound(response.Message);

            return Ok(response);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var response = await roleService.DeleteAsync(id, cancellationToken);
            if (!response)
            {
                return BadRequest("Failed to delete role");
            }

            return Ok("Role deleted successfully");
        }
    }
}
