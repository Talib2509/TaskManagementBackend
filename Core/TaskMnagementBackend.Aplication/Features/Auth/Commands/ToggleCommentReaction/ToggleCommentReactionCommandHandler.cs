using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Aplication.Features.Commands.ToggleCommentReaction
{
    public class ToggleCommentReactionCommandHandler : IRequestHandler<ToggleCommentReactionCommand, bool>
    {
        private readonly IReadRepository<CommentReaction> _reactionReadRepository;
        private readonly IWriteRepository<CommentReaction> _reactionWriteRepository;
        private readonly TaskMnagementBackend.Aplication.IUnitOfWork.IUnitOfWork _unitOfWork;

        public ToggleCommentReactionCommandHandler(
            IReadRepository<CommentReaction> reactionReadRepository,
            IWriteRepository<CommentReaction> reactionWriteRepository,
            TaskMnagementBackend.Aplication.IUnitOfWork.IUnitOfWork unitOfWork)
        {
            _reactionReadRepository = reactionReadRepository;
            _reactionWriteRepository = reactionWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ToggleCommentReactionCommand request, CancellationToken cancellationToken)
        {
           
            var existingReaction = await _reactionReadRepository.GetSingleAsync(r =>
                r.TaskCommentId == request.CommentId &&
                r.UserId == request.UserId &&
                r.Emoji == request.Emoji);

            if (existingReaction != null)
            {

                _reactionWriteRepository.Delete(existingReaction);
            }
            else
            {
                
                var newReaction = new CommentReaction
                {
                    TaskCommentId = request.CommentId,
                    UserId = request.UserId,
                    Emoji = request.Emoji,
                    CreatedAt = DateTime.UtcNow
                };

                await _reactionWriteRepository.AddAsync(newReaction);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}