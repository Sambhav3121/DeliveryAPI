using Microsoft.AspNetCore.Mvc;
using sambackend.Models;
using sambackend.Services; // Add the service namespace
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using sambackend.Dto;
namespace sambackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService )
        {
            _userService = userService; // Initialize IUserService
           
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var user = await _userService.RegisterUserAsync(registerDto);
                var token = _userService.GenerateJwtToken(user);

                return Ok(new 
                { 
                    status = "Success",
                    message = "Registration successful.",
                    token = token
                });
            }
            catch (Exception ex) when (ex.Message.Contains("Email is already in use"))
            {
                return Conflict(new { message = "Email is already in use." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during registration.", details = ex.Message });
            }
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.LoginUserAsync(loginDto);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var token = _userService.GenerateJwtToken(user);
            return Ok(new { token });
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // Simulate logout process (e.g., invalidate the session or token)
            return Ok(new
            {
                status = "Success",
                message = "You have been logged out successfully."
            });
        }
       
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out Guid userGuid))
                return BadRequest(new { message = "Invalid user ID." });

            try
            {
                var profile = await _userService.GetUserProfileAsync(userGuid);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
