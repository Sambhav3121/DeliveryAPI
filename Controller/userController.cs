using Microsoft.AspNetCore.Mvc;
using sambackend.Models;
using sambackend.Services; 
using sambackend.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using sambackend.Dto;

namespace sambackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(new 
        { 
            status = "error", 
            message = "Invalid registration data.", 
            errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) 
        });
    }

    try
    {
        var user = await _userService.RegisterUserAsync(registerDto);
        var token = await _userService.GenerateJwtTokenAsync(user);

        return Ok(new
        {
            status = "Success",
            message = "Registration successful.",
            user = new
            {
                fullName = user.FullName,
                email = user.Email,
                address = user.Address,
                birthDate = user.BirthDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), 
                gender = user.Gender,
                phoneNumber = user.PhoneNumber
            },
            token = token
        });
    }
    catch (InvalidEmailException ex)
    {
        return BadRequest(new { status = "error", message = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new { status = "error", message = ex.Message });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { status = "error", message = "An error occurred during registration.", details = ex.Message });
    }
}


        [HttpPost("login")]
      public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
   {
    if (!ModelState.IsValid)
    {
        return BadRequest(new 
        { 
            status = "error", 
            message = "Invalid login data.", 
            errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) 
        });
    }

    try
    {
        var user = await _userService.LoginUserAsync(loginDto);
        var token = await _userService.GenerateJwtTokenAsync(user); 

        return Ok(new 
        { 
            status = "Success",
            message = "Login successful.",
            token = token
        });
    }
    catch (InvalidCredentialsException)
    {
        return Unauthorized(new { message = "Invalid email or password." });
    }
    catch (InvalidEmailException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "An error occurred during login.", details = ex.Message });
    }
}


[HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User not logged in." });

                await _userService.LogoutUserAsync(Guid.Parse(userId));

                return Ok(new { status = "success", message = "User logged out successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during logout.", details = ex.Message });
            }
        }

[HttpGet("profile")]
[Authorize]
public async Task<IActionResult> GetUserProfile()
{
    var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (!Guid.TryParse(userIdClaim, out Guid userId))
    {
        return Unauthorized(new { message = "Invalid user ID." });
    }

    var userProfile = await _userService.GetUserProfileAsync(userId);

    if (userProfile == null)
    {
        return NotFound(new { status = "error", message = "User profile not found." });
    }

    return Ok(new
    {
        status = "success",
        data = new {
            id = userProfile.Id,
            fullName = userProfile.FullName,
            email = userProfile.Email,
            birthDate = userProfile.BirthDate,
            gender = userProfile.Gender,
            address = userProfile.Address,
            phoneNumber = userProfile.PhoneNumber
        }
    });
}
 
      [HttpPut("profile")]
      [Authorize]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileEdit userProfileEdit)
       {
          if (!ModelState.IsValid)
        return BadRequest(new { message = "Invalid input data." });

       string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        return Unauthorized(new { message = "Please log in to the system first." });

        try
        {
        var result = await _userService.UpdateUserProfileAsync(userProfileEdit);
        if (result == null)
            return NotFound(new { message = "User not found." });

        return Ok(result);
         }
         catch (InvalidOperationException ex)
        {
        return BadRequest(new { message = "Invalid operation: " + ex.Message });
         }
         catch (Exception ex)
        {
        return StatusCode(500, new { message = "An internal server error occurred.", details = ex.Message });
        }
    }


    }
}