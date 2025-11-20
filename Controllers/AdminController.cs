using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EShop.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class AdminController : ControllerBase
        {
            [Authorize(Roles = "Admin")]
            [HttpGet("admin-only")]
            public IActionResult AdminOnlyEndpoint()
            {
                Log.Information("Admin endpoint accessed by {User}", User.Identity?.Name);
                return Ok("You are an admin!");
            }
        }
    }
