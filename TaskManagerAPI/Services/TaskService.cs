using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync(
            string? sortBy, string? sortDir, string? filter, string? search)
        {
            var tasks = await _taskRepository.GetAllAsync(
                sortBy, sortDir, filter, search);
            return tasks.Select(t => MapToResponseDto(t));
        }

        public async Task<TaskResponseDto?> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;
            return MapToResponseDto(task);
        }

        public async Task<TaskResponseDto> CreateTaskAsync(TaskCreateDto dto)
        {
            var validPriorities = new[] { "Low", "Medium", "High" };
            if (!validPriorities.Contains(dto.Priority))
                dto.Priority = "Medium";

            var task = new TaskItem
            {
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim(),
                Priority = dto.Priority,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            var createdTask = await _taskRepository.CreateAsync(task);
            return MapToResponseDto(createdTask);
        }

        public async Task<TaskResponseDto?> UpdateTaskAsync(int id, TaskUpdateDto dto)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;

            var validPriorities = new[] { "Low", "Medium", "High" };
            if (!validPriorities.Contains(dto.Priority))
                dto.Priority = "Medium";

            task.Title = dto.Title.Trim();
            task.Description = dto.Description?.Trim();
            task.IsCompleted = dto.IsCompleted;
            task.Priority = dto.Priority;
            task.UpdatedAt = DateTime.UtcNow;

            var updatedTask = await _taskRepository.UpdateAsync(task);
            return MapToResponseDto(updatedTask);
        }

        public async Task<TaskResponseDto?> ToggleTaskAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;

            task.IsCompleted = !task.IsCompleted;
            task.UpdatedAt = DateTime.UtcNow;

            var updatedTask = await _taskRepository.UpdateAsync(task);
            return MapToResponseDto(updatedTask);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await _taskRepository.DeleteAsync(id);
        }

        private static TaskResponseDto MapToResponseDto(TaskItem task)
        {
            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }
    }
}