using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Services
{
    /// <summary>
    /// Handles business logic for managing tasks.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ITaskRepository taskRepository, ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all tasks from the database.
        /// </summary>
        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            _logger.LogInformation("Retrieving all tasks...");
            return await _taskRepository.GetAllTasksAsync();
        }

        /// <summary>
        /// Retrieves a task by its unique identifier.
        /// </summary>
        public async Task<TaskItem> GetTaskByIdAsync(string id)
        {
            _logger.LogInformation($"Retrieving task with ID: {id}");
            return await _taskRepository.GetTaskByIdAsync(id);
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        public async Task CreateTaskAsync(TaskItem task)
        {
            _logger.LogInformation($"Creating a new task: {task.Title}");
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;
            await _taskRepository.CreateTaskAsync(task);
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        public async Task<bool> UpdateTaskAsync(string id, TaskItem task)
        {
            _logger.LogInformation($"Updating task with ID: {id}");

            var existingTask = await _taskRepository.GetTaskByIdAsync(id);
            if (existingTask == null)
            {
                _logger.LogWarning($"Task with ID {id} not found.");
                return false;
            }

            task.Id = id; // Ensure the correct ID is assigned
            task.UpdatedAt = DateTime.UtcNow;

            await _taskRepository.UpdateTaskAsync(id, task);
            return true;
        }

        /// <summary>
        /// Deletes a task by its unique identifier.
        /// </summary>
        public async Task<bool> DeleteTaskAsync(string id)
        {
            _logger.LogInformation($"Deleting task with ID: {id}");

            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null)
            {
                _logger.LogWarning($"Task with ID {id} not found.");
                return false;
            }

            await _taskRepository.DeleteTaskAsync(id);
            return true;
        }
    }
}
