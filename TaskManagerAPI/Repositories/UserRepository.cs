
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories.Interfaces;

namespace TaskManagerAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByCredentialsAsync(string username, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Username == username && u.Password == password);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}