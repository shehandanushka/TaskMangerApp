using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories.Interfaces;

namespace TaskManagerAPI.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(
            string? sortBy, string? sortDir, string? filter, string? search)
        {
            var query = _context.Tasks.AsQueryable();

            // FILTER
            if (!string.IsNullOrEmpty(filter))
            {
                query = filter.ToLower() switch
                {
                    "completed" => query.Where(t => t.IsCompleted),
                    "pending" => query.Where(t => !t.IsCompleted),
                    "high" => query.Where(t => t.Priority == "High"),
                    "medium" => query.Where(t => t.Priority == "Medium"),
                    "low" => query.Where(t => t.Priority == "Low"),
                    _ => query
                };
            }

            // SEARCH
            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(t =>
                    t.Title.ToLower().Contains(searchLower) ||
                    (t.Description != null &&
                     t.Description.ToLower().Contains(searchLower)));
            }

            // SORT
            var ascending = sortDir?.ToLower() != "desc";
            query = sortBy?.ToLower() switch
            {
                "title" => ascending
                    ? query.OrderBy(t => t.Title)
                    : query.OrderByDescending(t => t.Title),
                "priority" => ascending
                    ? query.OrderBy(t => t.Priority)
                    : query.OrderByDescending(t => t.Priority),
                "status" => ascending
                    ? query.OrderBy(t => t.IsCompleted)
                    : query.OrderByDescending(t => t.IsCompleted),
                "created" => ascending
                    ? query.OrderBy(t => t.CreatedAt)
                    : query.OrderByDescending(t => t.CreatedAt),
                _ => query.OrderByDescending(t => t.CreatedAt)
            };

            return await query.ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem> UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Tasks.AnyAsync(t => t.Id == id);
        }
    }
}