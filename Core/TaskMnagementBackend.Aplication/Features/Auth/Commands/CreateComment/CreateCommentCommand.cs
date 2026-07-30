using MediatR;
using System;

namespace TaskMnagementBackend.Aplication.Features.Commands.CreateComment
{
    
    public class CreateCommentCommand : IRequest<bool>
    {
        public int TaskId { get; set; }
        public string Text { get; set; }
        public int? ParentCommentId { get; set; }
        public Guid UserId { get; set; }
    }
}