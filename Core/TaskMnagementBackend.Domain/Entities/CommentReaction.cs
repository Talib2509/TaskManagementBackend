using System;
using TaskMnagementBackend.Domain.Common;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Domain.Entities
{
    public class CommentReaction : BaseEntity 
    {
        
        public int TaskCommentId { get; set; } 
        public TaskComment TaskComment { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public string Emoji { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}