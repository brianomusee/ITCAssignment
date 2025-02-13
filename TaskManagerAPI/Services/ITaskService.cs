using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    /// <summary>
    /// Interface for task-related business logic operations.
    /// </summary>
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<TaskItem> GetTaskByIdAsync(string id);
        Task CreateTaskAsync(TaskItem task);
        Task<bool> UpdateTaskAsync(string id, TaskItem task);
        Task<bool> DeleteTaskAsync(string id);
    }
}
