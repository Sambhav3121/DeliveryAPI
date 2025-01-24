using Microsoft.AspNetCore.Mvc;
using sambackend.Models;
using sambackend.Services; 
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace sambackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
       public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(new { message = "Invalid registration data.", errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
    }

    try
    {
        var user = await _userService.RegisterUserAsync(registerDto);
        var token = await _userService.GenerateJwtTokenAsync(user);

        return Ok(new
        {
            status = "Success",
            message = "Registration successful.",
            token = token
        });
    }
    catch (InvalidEmailException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "An error occurred during registration.", details = ex.Message });
    }
}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _userService.LoginUserAsync(loginDto);
                var token = await _userService.GenerateJwtTokenAsync(user); 

                return Ok(new { token });
            }
            catch (InvalidCredentialsException)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during login.", details = ex.Message });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // Implement proper logout logic (e.g., invalidate tokens in a distributed cache)
            return Ok(new
            {
                status = "Success",
                message = "You have been logged out successfully."
            });
        }

       
        
    }
}