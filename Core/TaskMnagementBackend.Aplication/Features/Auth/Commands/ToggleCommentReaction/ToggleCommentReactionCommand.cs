using MediatR;
using System;

namespace TaskMnagementBackend.Aplication.Features.Commands.ToggleCommentReaction
{
    public class ToggleCommentReactionCommand : IRequest<bool>
    {
        public int CommentId { get; set; }
        public string Emoji { get; set; } 
        public Guid UserId { get; set; } 
    }
}