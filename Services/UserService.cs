using sambackend.Data; 
using sambackend.Models; // To resolve RegisterDto and User
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BCrypt.Net;
using System.Security.Claims; // For Claim
using Microsoft.IdentityModel.Tokens; // For SigningCredentials, SecurityAlgorithms
using System.Text; // For Encoding
using System.IdentityModel.Tokens.Jwt; // For JwtSecurityToken, JwtSecurityTokenHandler

namespace sambackend.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterUserAsync(RegisterDto registerDto)
        {
            // Check if the email already exists
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                throw new Exception("Email is already in use.");
            }

            // Hash the password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Create a new user
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

            return GenerateJwtToken(user);
        }

        // Method for generating JWT Token
        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("fullName", user.FullName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-jwt-secret-key"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "your-issuer",
                audience: "your-audience",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    public async Task<User> LoginUserAsync(LoginDto loginDto)
   {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
    if (user == null)
    {
        throw new Exception("Invalid Email or Password.");
    }
    if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
    {
        throw new Exception("Invalid Email or Password.");
    }

    string token = GenerateJwtToken(user);
    var storeToken = new storeToken
    {
        Id = Guid.NewGuid(),
        Email = user.Email,
        Token = token
    };
    _context.storeToken.Add(storeToken);
    await _context.SaveChangesAsync();

    return user;
}

      public async Task<GetUserProfile> GetUserProfileAsync(int userId)
    {
       var user = await _context.Users.FindAsync(userId);
     if (user == null)
     {
        throw new Exception("User not found.");
     }
      return new GetUserProfile
      {
        FullName = user.FullName,
        Email = user.Email,
        BirthDate = user.BirthDate,
        Gender = user.Gender,
        Address = user.Address,
        PhoneNumber = user.PhoneNumber
       };
     

        } 

    }
}
