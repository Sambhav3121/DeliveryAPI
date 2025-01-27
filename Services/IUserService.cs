using sambackend.Dto;
using sambackend.Models;
using System.Threading.Tasks;

namespace sambackend.Services
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(RegisterDto registerDto);
       Task<string> GenerateJwtTokenAsync(User user); 
        Task<User> LoginUserAsync(LoginDto loginDto);
        Task<User> GetUserProfileAsync(Guid userId);
        Task<UserResponse> UpdateUserProfileAsync( UserProfileEdit userProfileEdit);

    }
}
