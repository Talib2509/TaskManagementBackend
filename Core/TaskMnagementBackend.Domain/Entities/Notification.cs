using System;
using TaskMnagementBackend.Domain.Common;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Domain.Entities
{
    public class Notification : BaseEntity
    {
        
        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } 

        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}