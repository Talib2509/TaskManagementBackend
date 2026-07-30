using System;

namespace TaskMnagementBackend.Aplication.DTOs
{
    public class TaskTimelineDto
    {
        public int Id { get; set; }
        public int ProjectTaskId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string ActionType { get; set; } 
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}