using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using TaskManagerAPI.Enums;

namespace TaskManagerAPI.Models
{
    /// <summary>
    /// Represents a task item in the system.
    /// </summary>
    public class TaskItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("status")]
        public Enums.TaskStatus Status { get; set; }

        [BsonElement("priority")]
        public TaskPriority Priority { get; set; }

        [BsonElement("dueDate")]
        public DateTime DueDate { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
