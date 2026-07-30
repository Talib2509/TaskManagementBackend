using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Aplication.Features.Commands.UploadTaskAttachment
{
    public class UploadChunkCommandHandler : IRequestHandler<UploadChunkCommand, bool>
    {
        private readonly IWriteRepository<TaskAttachment> _attachmentWriteRepository;
        private readonly IStorageService _storageService;
        private readonly TaskMnagementBackend.Aplication.IUnitOfWork.IUnitOfWork _unitOfWork;

        public UploadChunkCommandHandler(
            IWriteRepository<TaskAttachment> attachmentWriteRepository,
            IStorageService storageService,
            TaskMnagementBackend.Aplication.IUnitOfWork.IUnitOfWork unitOfWork)
        {
            _attachmentWriteRepository = attachmentWriteRepository;
            _storageService = storageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UploadChunkCommand request, CancellationToken cancellationToken)
        {
            var extension = Path.GetExtension(request.FileName).ToLower();

           
            string uniqueFileName = $"{request.FileGuid}{extension}";

            
            var filePath = await _storageService.AppendChunkAsync(request.Chunk, uniqueFileName, "task_attachments");

           
            if (request.ChunkIndex == request.TotalChunks - 1)
            {
                var attachment = new TaskAttachment
                {
                    OriginalFileName = request.FileName,
                    StoredFileName = Path.GetFileName(filePath),
                    Extension = extension,
                    MimeType = request.Chunk.ContentType, 
                    SizeInBytes = 0, 
                    FilePath = filePath,
                    ThumbnailPath = null, 
                    ProjectTaskId = request.TaskId,
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow
                };

                await _attachmentWriteRepository.AddAsync(attachment);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return true;
        }
    }
}