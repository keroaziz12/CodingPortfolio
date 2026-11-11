using CodingPortfolio.DTOs;
using CodingPortfolio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodingPortfolio.Controllers
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
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var developer = await _authService.RegisterAsync(request.Name, request.Email, request.Password);
            if (developer is null)

            return Conflict(new { message = "Email already registered" });

            var response = new DeveloperResponse(developer.DeveloperId, developer.Name, developer.Email, developer.CreatedAt);

            return StatusCode(201, response);


        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);
            if (token is null)
                return Unauthorized(new { message = "Invalid email or password" });
            var response = new AuthResponse(token);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var id =User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(new {id, email });
        }
    }
}
