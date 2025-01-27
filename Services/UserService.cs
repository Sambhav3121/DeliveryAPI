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
            if (string.IsNullOrEmpty(registerDto.Email) || string.IsNullOrEmpty(registerDto.Password) || string.IsNullOrEmpty(registerDto.FullName))
            {
                throw new ArgumentException("Missing required fields.");
            }

            if (!IsValidEmail(registerDto.Email))
            {
                throw new InvalidEmailException("Invalid email format.");
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

          Console.WriteLine($"Generating token for user: {user.Id} (Type: {user.Id.GetType()})"); // Debugging

           var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),  // Make sure this is the first claim
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning($"Failed login attempt for email: {loginDto.Email}");
                throw new InvalidCredentialsException("Invalid email or password.");
            }

            _logger.LogInformation($"User logged in: {user.Email}");
            return user;
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

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
