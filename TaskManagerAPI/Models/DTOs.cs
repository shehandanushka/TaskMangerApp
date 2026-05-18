using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class TaskCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public string Priority { get; set; } = "Medium";
    }

    public class TaskUpdateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public string Priority { get; set; } = "Medium";
    }

    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public string Priority { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }

    public class ApiResponse
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}