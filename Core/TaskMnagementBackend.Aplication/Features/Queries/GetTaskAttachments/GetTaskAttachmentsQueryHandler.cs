using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.DTOs;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Aplication.Features.Queries.GetTaskAttachments
{
    public class GetTaskAttachmentsQueryHandler : IRequestHandler<GetTaskAttachmentsQuery, List<TaskAttachmentDto>>
    {
        private readonly IReadRepository<TaskAttachment> _attachmentReadRepository;
        private readonly IStorageService _storageService;

        public GetTaskAttachmentsQueryHandler(
            IReadRepository<TaskAttachment> attachmentReadRepository,
            IStorageService storageService)
        {
            _attachmentReadRepository = attachmentReadRepository;
            _storageService = storageService;
        }

        public async Task<List<TaskAttachmentDto>> Handle(GetTaskAttachmentsQuery request, CancellationToken cancellationToken)
        {
            var attachments = await _attachmentReadRepository
                .GetWhere(a => a.ProjectTaskId == request.TaskId && !a.IsDeleted)
                .Include(a => a.User)
                .Select(a => new TaskAttachmentDto
                {
                    Id = a.Id,
                    OriginalFileName = a.OriginalFileName,
                    Extension = a.Extension,
                    SizeInBytes = a.SizeInBytes,
                    
                    FileUrl = $"/api/attachments/download/{a.Id}",
                    ThumbnailUrl = a.ThumbnailPath != null ? _storageService.GetFileUrl(a.ThumbnailPath, "") : null,
                    UserId = a.UserId,
                    UserName = a.User.UserName,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return attachments;
        }
    }
}