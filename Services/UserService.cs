
using sambackend.Data; 
using sambackend.Models; 
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Claims; 
using Microsoft.IdentityModel.Tokens; 
using System.Text; 
using System.IdentityModel.Tokens.Jwt; 
using Microsoft.Extensions.Options;
using BCrypt.Net;
namespace sambackend.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly JwtSettings _jwtSettings; 

        public UserService(DataContext context, IOptions<JwtSettings> jwtSettings) 
        {
            _context = context;
            _jwtSettings = jwtSettings.Value; 
        }

        public async Task<User> RegisterUserAsync(RegisterDto registerDto)
        {
            if (string.IsNullOrEmpty(registerDto.Email) || string.IsNullOrEmpty(registerDto.Password) 
                || string.IsNullOrEmpty(registerDto.FullName)) 
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

          
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password); // Use BCrypt for hashing

            var user = new User
            {
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
            return user;
        }

      public Task<string> GenerateJwtTokenAsync(User user)
    {
      var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("fullName", user.FullName)
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        claims: claims,
        expires: DateTime.Now.AddHours(1),
        signingCredentials: creds
    );

    return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
}

        public async Task<User> LoginUserAsync(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                throw new InvalidCredentialsException("Invalid email or password.");
            }

            // Verify password using Argon2
            if(!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new InvalidCredentialsException("Invalid email or password.");
            }

            return user;
        }


        private bool IsValidEmail(string email)
        {
            // Add your email validation logic here
            // For example, use a regular expression or a dedicated email validation library
            return true; 
        }
    }

    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(string message) : base(message) { }
    }

    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string message) : base(message) { }
    }
}
