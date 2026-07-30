using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Aplication.Features.Commands.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, bool>
    {
        private readonly IReadRepository<TaskComment> _commentReadRepository;
        private readonly IWriteRepository<TaskComment> _commentWriteRepository;
        private readonly TaskMnagementBackend.Aplication.IUnitOfWork.IUnitOfWork _unitOfWork;

        public DeleteCommentCommandHandler(
            IReadRepository<TaskComment> commentReadRepository,
            IWriteRepository<TaskComment> commentWriteRepository,
            TaskMnagementBackend.Aplication.IUnitOfWork.IUnitOfWork unitOfWork)
        {
            _commentReadRepository = commentReadRepository;
            _commentWriteRepository = commentWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentReadRepository.GetByIdAsync(request.CommentId);

            if (comment == null || comment.IsDeleted)
                throw new Exception("Şərh tapılmadı və ya artıq silinib.");

            
            if (comment.UserId != request.UserId)
                throw new Exception("Sizin bu şərhi silməyə icazəniz yoxdur.");

            
            comment.IsDeleted = true;

            _commentWriteRepository.Update(comment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}