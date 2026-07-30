using MediatR;
using System.Collections.Generic;
using TaskMnagementBackend.Aplication.DTOs;

namespace TaskMnagementBackend.Aplication.Features.Queries.GetTaskAttachments
{
    public class GetTaskAttachmentsQuery : IRequest<List<TaskAttachmentDto>>
    {
        public int TaskId { get; set; }
    }
}