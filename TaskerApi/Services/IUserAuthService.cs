using TaskerApi.DTOS;
using TaskerApi.Models;

namespace TaskerApi.Services
{
    public interface IUserAuthService
    {
        Task<string> CreateAndSaveRefreshToken(User user);
        Task<string> CreateToken(User user);
        Task<ServiceResult<LoginResponseDto>> Login(LoginRequestDto request);
        Task<ServiceResult<string>> Logout(int id);
        Task<ServiceResult<RefreshTokenResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task<ServiceResult<RegisterResponseDto>> Register(RegisterRequestDto request);
    }
}