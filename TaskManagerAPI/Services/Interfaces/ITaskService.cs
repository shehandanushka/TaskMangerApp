using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync(
            string? sortBy, string? sortDir, string? filter, string? search);
        Task<TaskResponseDto?> GetTaskByIdAsync(int id);
        Task<TaskResponseDto> CreateTaskAsync(TaskCreateDto dto);
        Task<TaskResponseDto?> UpdateTaskAsync(int id, TaskUpdateDto dto);
        Task<TaskResponseDto?> ToggleTaskAsync(int id);
        Task<bool> DeleteTaskAsync(int id);
    }
}