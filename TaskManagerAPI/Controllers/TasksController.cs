using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks(
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDir,
            [FromQuery] string? filter,
            [FromQuery] string? search)
        {
            var tasks = await _taskService.GetAllTasksAsync(
                sortBy, sortDir, filter, search);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponseDto>> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound(new ApiResponse
                { Message = "Task not found", Success = false });
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> CreateTask(
            [FromBody] TaskCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = await _taskService.CreateTaskAsync(dto);
            return CreatedAtAction(nameof(GetTask),
                new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskResponseDto>> UpdateTask(
            int id, [FromBody] TaskUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = await _taskService.UpdateTaskAsync(id, dto);
            if (task == null)
                return NotFound(new ApiResponse
                { Message = "Task not found", Success = false });
            return Ok(task);
        }

        [HttpPatch("{id}/toggle")]
        public async Task<ActionResult<TaskResponseDto>> ToggleTask(int id)
        {
            var task = await _taskService.ToggleTaskAsync(id);
            if (task == null)
                return NotFound(new ApiResponse
                { Message = "Task not found", Success = false });
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (!result)
                return NotFound(new ApiResponse
                { Message = "Task not found", Success = false });
            return Ok(new ApiResponse
            { Message = "Task deleted successfully", Success = true });
        }
    }
}