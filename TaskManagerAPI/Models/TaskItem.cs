using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [Required]
        public string Priority { get; set; } = "Medium";
    }
}