using MediatR;
using System;

namespace TaskMnagementBackend.Aplication.Features.Commands.UpdateComment
{
    public class UpdateCommentCommand : IRequest<bool>
    {
        public int CommentId { get; set; }
        public string NewText { get; set; }
        public Guid UserId { get; set; } 
    }
}