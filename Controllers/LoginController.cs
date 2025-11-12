using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EShop.Controllers
{
    public class LoginControl
    {
        [ApiController]
        [Route("api/[controller]")]
        public class LoginController : ControllerBase
        {
            [HttpPost("login")]
            public IActionResult Login([FromBody] LoginRequest request)
            {
                
                Log.Information("Login attempt received for user: {Email}", request.Email);

                if (request.Email == "admin@example.com" && request.Password == "password123")
                {
                    Log.Information("Login successful for {Email}", request.Email);
                    return Ok(new { message = "Login successful!" });
                }

                Log.Warning("Invalid login credentials for {Email}", request.Email);
                return Unauthorized(new { message = "Invalid email or password." });
            }
        }
    }
}
