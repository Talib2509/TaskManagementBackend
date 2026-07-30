using System;
using TaskMnagementBackend.Domain.Common;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Domain.Entities
{
    public class TaskAttachment : BaseEntity
    {
        public string OriginalFileName { get; set; } 
        public string StoredFileName { get; set; }   
        public string Extension { get; set; }        
        public string MimeType { get; set; }       
        public long SizeInBytes { get; set; }        
        public string FilePath { get; set; }         
        public string? ThumbnailPath { get; set; }   

        public int ProjectTaskId { get; set; }
        

        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}