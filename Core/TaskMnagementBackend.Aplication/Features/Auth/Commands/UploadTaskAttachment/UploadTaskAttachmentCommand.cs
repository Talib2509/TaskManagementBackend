using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace TaskMnagementBackend.Aplication.Features.Commands.UploadTaskAttachment
{
    public class UploadTaskAttachmentCommand : IRequest<bool>
    {
        public int TaskId { get; set; }
        public Guid UserId { get; set; }
        public IFormFile File { get; set; }
    }
}