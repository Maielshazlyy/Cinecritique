using CineCritique.DAL.DTOS;
using CineCritique.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineCritique.Api.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AuthController:Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _authService.LoginAsync(loginDto);
            if (token == null) return Unauthorized("Invalid username or password");

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var success = await _authService.RegisterAsync(registerDto);
            if (!success) return BadRequest("User already exists or invalid data.");

            return Ok("User registered successfully.");
        }
    }
}
