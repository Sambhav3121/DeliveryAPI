using sambackend.Models;
using System.Threading.Tasks;

namespace sambackend.Services
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(RegisterDto registerDto);
         string GenerateJwtToken(User user); 
        Task<User> LoginUserAsync(LoginDto loginDto);
         Task<GetUserProfile> GetUserProfileAsync(int userId); 
    }
}
