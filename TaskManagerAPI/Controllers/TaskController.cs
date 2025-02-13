using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;
using Microsoft.Extensions.Logging;

namespace TaskManagerAPI.Controllers
{
    /// <summary>
    /// API Controller for managing tasks.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskRepository taskRepository, IMapper mapper, ILogger<TaskController> logger)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all tasks.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAllTasks()
        {
            _logger.LogInformation("Fetching all tasks...");
            try
            {
                var tasks = await _taskRepository.GetAllTasksAsync();
                var taskDtos = _mapper.Map<IEnumerable<TaskResponseDto>>(tasks);
                return Ok(taskDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all tasks.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Retrieves a task by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponseDto>> GetTaskById(string id)
        {
            _logger.LogInformation($"Fetching task with ID: {id}");
            try
            {
                var task = await _taskRepository.GetTaskByIdAsync(id);
                if (task == null) return NotFound(new { message = "Task not found." });
                var taskDto = _mapper.Map<TaskResponseDto>(task);
                return Ok(taskDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching task {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// 
        // Creates a new task.
        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> CreateTask([FromBody] TaskCreateDto taskDto)
        {
            _logger.LogInformation("Creating a new task...");
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    _logger.LogError($"Validation failed: {errors}");
                    return BadRequest(new { message = errors });
                }

                var task = _mapper.Map<TaskItem>(taskDto);
                task.CreatedAt = task.UpdatedAt = System.DateTime.UtcNow;
                await _taskRepository.CreateTaskAsync(task);
                var taskReadDto = _mapper.Map<TaskResponseDto>(task);
                return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, taskReadDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new task.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Updates an existing task.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(string id, TaskUpdateDto taskDto)
        {
            _logger.LogInformation($"Updating task with ID: {id}");
            try
            {
                var existingTask = await _taskRepository.GetTaskByIdAsync(id);
                if (existingTask == null) return NotFound(new { message = "Task not found." });

                _mapper.Map(taskDto, existingTask);
                existingTask.UpdatedAt = System.DateTime.UtcNow;
                await _taskRepository.UpdateTaskAsync(id, existingTask);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating task {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Soft deletes a task.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            _logger.LogInformation($"Soft deleting task with ID: {id}");
            try
            {
                bool deleted = await _taskRepository.DeleteTaskAsync(id);
                if (!deleted) return NotFound(new { message = "Task not found." });
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting task {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }



    }
}
