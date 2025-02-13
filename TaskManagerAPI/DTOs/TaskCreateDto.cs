using System;
using System.ComponentModel.DataAnnotations;
using TaskManagerAPI.Enums;

namespace TaskManagerAPI.DTOs
{
    /// <summary>
    /// DTO for creating a new task.
    /// </summary>
    public class TaskCreateDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [EnumDataType(typeof(Enums.TaskStatus), ErrorMessage = "Invalid status value.")]
        public Enums.TaskStatus Status { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        [EnumDataType(typeof(TaskPriority), ErrorMessage = "Invalid priority value.")]
        public TaskPriority Priority { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime DueDate { get; set; }
    }
}
