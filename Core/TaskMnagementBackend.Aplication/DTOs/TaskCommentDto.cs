using System;
using System.Collections.Generic;

namespace TaskMnagementBackend.Aplication.DTOs
{
    public class TaskCommentDto
    {
        public int Id { get; set; } 
        public string Text { get; set; }
        public int? ParentCommentId { get; set; } 
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public Dictionary<string, int> Reactions { get; set; } = new Dictionary<string, int>();
        public List<TaskCommentDto> Replies { get; set; } = new List<TaskCommentDto>();
    }
}