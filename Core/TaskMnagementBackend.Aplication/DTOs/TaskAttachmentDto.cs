using System;

namespace TaskMnagementBackend.Aplication.DTOs
{
    public class TaskAttachmentDto
    {
        public int Id { get; set; }
        public string OriginalFileName { get; set; }
        public string Extension { get; set; }
        public long SizeInBytes { get; set; }
        public string FileUrl { get; set; }      
        public string? ThumbnailUrl { get; set; } 
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}