using MediatR;
using System;

namespace TaskMnagementBackend.Aplication.Features.Commands.DeleteTaskAttachment
{
    public class DeleteTaskAttachmentCommand : IRequest<bool>
    {
        public int AttachmentId { get; set; }
        public Guid UserId { get; set; }
        
    }
}