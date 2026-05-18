using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync(
            string? sortBy, string? sortDir, string? filter, string? search);
        Task<TaskItem?> GetByIdAsync(int id);
        Task<TaskItem> CreateAsync(TaskItem task);
        Task<TaskItem> UpdateAsync(TaskItem task);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}