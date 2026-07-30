using MediatR;
using System;

namespace TaskMnagementBackend.Aplication.Features.Commands.DeleteComment
{
    public class DeleteCommentCommand : IRequest<bool>
    {
        public int CommentId { get; set; }
        public Guid UserId { get; set; } 
    }
}