using sambackend.Data;
using sambackend.Models;
using sambackend.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using BCrypt.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using sambackend.Dto;
using System.ComponentModel.DataAnnotations;

namespace sambackend.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<UserService> _logger;

        public UserService(DataContext context, IOptions<JwtSettings> jwtSettings, ILogger<UserService> logger)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public async Task<User> RegisterUserAsync(RegisterDto registerDto)
        {
            if (string.IsNullOrWhiteSpace(registerDto.Email) ||
                string.IsNullOrWhiteSpace(registerDto.Password) ||
                string.IsNullOrWhiteSpace(registerDto.FullName))
            {
                throw new ArgumentException("Missing required fields.");
            }

            if (!new EmailAddressAttribute().IsValid(registerDto.Email))
            {
                throw new InvalidEmailException("Invalid email format.");
            }

            if (!Regex.IsMatch(registerDto.Gender, "^(Male|Female|Other)$"))
            {
                throw new ArgumentException("Gender must be 'Male', 'Female', or 'Other'.");
            }

            if (!Regex.IsMatch(registerDto.Password, @"^(?=.*\d).{6,}$"))
            {
                throw new ArgumentException("Password must be at least 6 characters long and contain at least one numeric digit.");
            }

            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                throw new Exception("Email is already in use.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Address = registerDto.Address,
                BirthDate = registerDto.BirthDate,
                Gender = registerDto.Gender,
                PhoneNumber = registerDto.PhoneNumber
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User registered successfully: {user.Email}");
            return user;
        }

        public Task<string> GenerateJwtTokenAsync(User user)
        {
            if (user == null || user.Id == Guid.Empty)
            {
                throw new ArgumentException("Invalid user data for token generation.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            _logger.LogInformation($"Generated token for user ID: {user.Id}");
            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<User> LoginUserAsync(LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                throw new ArgumentException("Email and Password are required.");
            }

            if (!new EmailAddressAttribute().IsValid(loginDto.Email))
            {
                throw new InvalidEmailException("Invalid email format.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning($"Failed login attempt for email: {loginDto.Email}");
                throw new InvalidCredentialsException("Invalid email or password.");
            }

            _logger.LogInformation($"User logged in: {user.Email}");
            return user;
        }

        public async Task LogoutUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found.");
        }

        public async Task<User> GetUserProfileAsync(Guid userId)
        {
            _logger.LogInformation($"Fetching user profile for ID: {userId}");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"User profile not found for ID: {userId}");
                return null;
            }

            return user;
        }

       
        public async Task<UserResponse> UpdateUserAsync(Guid userId, UserProfileEdit userProfileEdit)
        {
            var userProfile = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userProfile == null)
            {
                return new UserResponse
                {
                    Status = "Failure",
                    Message = "User not found"
                };
            }

        
            userProfile.FullName = userProfileEdit.FullName;
            userProfile.Address = userProfileEdit.Address;
            userProfile.BirthDate = userProfileEdit.BirthDate;
            userProfile.PhoneNumber = userProfileEdit.PhoneNumber;

            await _context.SaveChangesAsync();

            return new UserResponse
            {
                Status = "Success",
                Message = "User updated successfully."
            };
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {userId} not found.");
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User with ID {userId} deleted successfully.");
            return true;
        }
    }
}
