using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByCredentialsAsync(string username, string password);
        Task<User?> GetByUsernameAsync(string username);
    }
}