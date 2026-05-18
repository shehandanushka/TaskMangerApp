using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto dto);
        Task<User?> ValidateCredentialsAsync(string username, string password);
    }
}