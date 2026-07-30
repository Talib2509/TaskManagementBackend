using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace TaskMnagementBackend.Aplication.Features.Commands.UploadTaskAttachment
{
    public class UploadChunkCommand : IRequest<bool>
    {
        public int TaskId { get; set; }
        public Guid UserId { get; set; }
        public IFormFile Chunk { get; set; } 
        public string FileGuid { get; set; } 
        public string FileName { get; set; } 
        public int ChunkIndex { get; set; }  
        public int TotalChunks { get; set; } 
    }
}