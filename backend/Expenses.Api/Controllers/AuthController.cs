using Expenses.Api.Dtos;
using Expenses.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await this._authService.RegisterAsync(userDto);
                var token = this._authService.GenerateToken(user);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to register user {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _authService.LoginAsync(loginUserDto);
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }
            var token = _authService.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }
}
