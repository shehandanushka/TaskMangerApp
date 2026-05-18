using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(dto);

            if (result == null)
                return Unauthorized(new ApiResponse
                {
                    Message = "Invalid username or password",
                    Success = false
                });

            return Ok(result);
        }
    }
}