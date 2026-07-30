using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.DTOs;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Aplication.Features.Queries.GetTaskComments
{
    public class GetTaskCommentsQueryHandler : IRequestHandler<GetTaskCommentsQuery, List<TaskCommentDto>>
    {
        private readonly IReadRepository<TaskComment> _commentReadRepository;

        public GetTaskCommentsQueryHandler(IReadRepository<TaskComment> commentReadRepository)
        {
            _commentReadRepository = commentReadRepository;
        }

        public async Task<List<TaskCommentDto>> Handle(GetTaskCommentsQuery request, CancellationToken cancellationToken)
        {
            
            var allComments = await _commentReadRepository.GetAll()
                .Include(c => c.User)
                .Include(c => c.Reactions)
                .Where(c => c.ProjectTaskId == request.TaskId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync(cancellationToken);

            List<TaskCommentDto> BuildCommentTree(List<TaskComment> comments, int? parentId)
            {
                return comments
                    .Where(c => c.ParentCommentId == parentId)
                    .Select(c => new TaskCommentDto
                    {
                        Id = c.Id,
                        Text = c.IsDeleted ? " Bu şərh silinib." : c.Text,
                        ParentCommentId = c.ParentCommentId,
                        UserId = c.UserId,
                        UserName = c.User?.UserName,
                        CreatedAt = c.CreatedAt,
                        IsDeleted = c.IsDeleted,
                        Reactions = c.Reactions.GroupBy(r => r.Emoji)
                                               .ToDictionary(g => g.Key, g => g.Count()),
                        Replies = BuildCommentTree(comments, c.Id)
                    })
                    .ToList();
            }

            return BuildCommentTree(allComments, null);
        }
    }
}