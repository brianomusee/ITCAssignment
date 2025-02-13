using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    /// <summary>
    /// Interface for task repository following the repository pattern.
    /// </summary>
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();   //  Get all non-deleted tasks
        Task<TaskItem> GetTaskByIdAsync(string id);       // Get a task by ID
        Task CreateTaskAsync(TaskItem task);             // Create a new task
        Task UpdateTaskAsync(string id, TaskItem task);  //  Update an existing task
        Task<bool> DeleteTaskAsync(string id);           // Soft delete (marks task as deleted)
    }
}
