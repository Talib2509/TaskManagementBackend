using System;
using System.Collections.Generic;
using TaskMnagementBackend.Domain.Common; 
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Domain.Entities
{
    public class TaskComment : BaseEntity 
    {
        

        public string Text { get; set; }

        public int ProjectTaskId { get; set; } 

        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public int? ParentCommentId { get; set; } 
        public TaskComment ParentComment { get; set; }
        public ICollection<TaskComment> Replies { get; set; } = new List<TaskComment>();

        public ICollection<CommentReaction> Reactions { get; set; } = new List<CommentReaction>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}