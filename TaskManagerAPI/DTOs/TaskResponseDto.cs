using System;

namespace TaskManagerAPI.DTOs
{
    /// <summary>
    /// DTO for returning task data to the client.
    /// </summary>
    public class TaskResponseDto
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public string Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
