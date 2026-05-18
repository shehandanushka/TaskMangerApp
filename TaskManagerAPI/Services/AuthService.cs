using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByCredentialsAsync(
                dto.Username, dto.Password);

            if (user == null) return null;

            return new LoginResponseDto
            {
                Message = "Login successful",
                Username = user.Username
            };
        }

        public async Task<User?> ValidateCredentialsAsync(
            string username, string password)
        {
            return await _userRepository.GetByCredentialsAsync(
                username, password);
        }
    }
}