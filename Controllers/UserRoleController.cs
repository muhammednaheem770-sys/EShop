using EShop.entities;
using EShop.service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EShop.Dto.Auth;
using System;

namespace EShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUser([FromQuery] Guid userId, [FromQuery] Guid roleId, CancellationToken cancellationToken)
        {
            if (userId == Guid.Empty || roleId == Guid.Empty)
                return BadRequest("UserId and RoleId must be valid GUIDs.");

            var response = await _userRoleService.AssignRoleToUserAsync(userId, roleId, cancellationToken);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("get-roles-by-user/{userId:guid}")]
        public async Task<IActionResult> GetRolesByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var response = await _userRoleService.GetRolesByUserIdAsync(userId, cancellationToken);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("get-users-by-role/{roleId:guid}")]
        public async Task<IActionResult> GetUsersByRoleId(Guid roleId, CancellationToken cancellationToken)
        {
            var response = await _userRoleService.GetUsersByRoleIdAsync(roleId, cancellationToken);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("assign-role/{userId:guid}")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request, CancellationToken cancellationToken)
        {
            var response = await _userRoleService.AssignRoleAsync(request.UserId, request.RoleName, cancellationToken);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
