using System;
using TaskMnagementBackend.Domain.Common; 
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Domain.Entities
{
    public class TaskActivityLog : BaseEntity 
    {


        public int ProjectTaskId { get; set; } 

        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public string ActionType { get; set; } 
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}