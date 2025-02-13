using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TaskManagerAPI.Repositories
{
    /// <summary>
    /// Repository for managing task data in MongoDB.
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private readonly IMongoCollection<TaskItem> _tasks;
        private readonly ILogger<TaskRepository> _logger;

        public TaskRepository(IConfiguration config, ILogger<TaskRepository> logger)
        {
            _logger = logger;
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _tasks = database.GetCollection<TaskItem>("Tasks");
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            _logger.LogInformation("Fetching all tasks...");
            return await _tasks.Find(task => !task.IsDeleted).ToListAsync();
        }

        public async Task<TaskItem> GetTaskByIdAsync(string id)
        {
            _logger.LogInformation($"Fetching task with ID: {id}");
            return await _tasks.Find(task => task.Id == id && !task.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task CreateTaskAsync(TaskItem task)
        {
            _logger.LogInformation("Creating a new task...");
            await _tasks.InsertOneAsync(task);
        }

        public async Task UpdateTaskAsync(string id, TaskItem task)
        {
            _logger.LogInformation($"Updating task with ID: {id}");
            await _tasks.ReplaceOneAsync(t => t.Id == id, task);
        }

        public async Task<bool> DeleteTaskAsync(string id)
        {
            _logger.LogInformation($"Soft deleting task with ID: {id}");
            var update = Builders<TaskItem>.Update.Set(t => t.IsDeleted, true);
            var result = await _tasks.UpdateOneAsync(t => t.Id == id, update);
            return result.ModifiedCount > 0;
        }
    }
}
