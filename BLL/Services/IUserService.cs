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
    Task LogoutUserAsync(Guid userId);
    Task<User> GetUserProfileAsync(Guid userId);
    Task<UserResponse> UpdateUserAsync(Guid userId, UserProfileEdit userProfileEdit);  // Add this method
    Task<bool> DeleteUserAsync(Guid userId);
}

}
