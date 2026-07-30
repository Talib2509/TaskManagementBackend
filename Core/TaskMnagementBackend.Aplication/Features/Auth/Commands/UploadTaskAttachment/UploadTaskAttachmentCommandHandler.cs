using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Aplication.Features.Commands.UploadTaskAttachment
{
    public class UploadTaskAttachmentCommandHandler : IRequestHandler<UploadTaskAttachmentCommand, bool>
    {
        private readonly IWriteRepository<TaskAttachment> _attachmentWriteRepository;
        private readonly IReadRepository<TaskAttachment> _attachmentReadRepository;
        private readonly IStorageService _storageService;
        private readonly TaskMnagementBackend.Aplication.IUnitOfWork.IUnitOfWork _unitOfWork;

        
        private const long MAX_IMAGE_SIZE = 10 * 1024 * 1024; 
        private const long MAX_DOCUMENT_SIZE = 20 * 1024 * 1024; 
        private const long MAX_VIDEO_SIZE = 100 * 1024 * 1024; 
        private const int MAX_FILES_PER_TASK = 10; 
        private const long MAX_TOTAL_SIZE_PER_TASK = 150 * 1024 * 1024; 

        public UploadTaskAttachmentCommandHandler(
            IWriteRepository<TaskAttachment> attachmentWriteRepository,
            IReadRepository<TaskAttachment> attachmentReadRepository,
            IStorageService storageService,
            TaskMnagementBackend.Aplication.IUnitOfWork.IUnitOfWork unitOfWork)
        {
            _attachmentWriteRepository = attachmentWriteRepository;
            _attachmentReadRepository = attachmentReadRepository;
            _storageService = storageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UploadTaskAttachmentCommand request, CancellationToken cancellationToken)
        {
            var extension = Path.GetExtension(request.File.FileName).ToLower();
            var incomingFileSize = request.File.Length;

            
            if (extension == ".jpg" || extension == ".png" || extension == ".jpeg" || extension == ".webp")
            {
                if (incomingFileSize > MAX_IMAGE_SIZE)
                    throw new Exception("Şəkillər maksimum 10 MB ola bilər.");
            }
            else if (extension == ".pdf" || extension == ".docx" || extension == ".xlsx" || extension == ".txt")
            {
                if (incomingFileSize > MAX_DOCUMENT_SIZE)
                    throw new Exception("Sənədlər maksimum 20 MB ola bilər.");
            }
            else if (extension == ".mp4" || extension == ".mov")
            {
                if (incomingFileSize > MAX_VIDEO_SIZE)
                    throw new Exception("Videolar maksimum 100 MB ola bilər.");
            }
            else
            {
                throw new Exception("Dəstəklənməyən fayl formatı.");
            }

            
            var existingAttachments = await _attachmentReadRepository
                .GetWhere(a => a.ProjectTaskId == request.TaskId && !a.IsDeleted)
                .ToListAsync(cancellationToken);

            if (existingAttachments.Count >= MAX_FILES_PER_TASK)
            {
                throw new Exception($"Bu tapşırığa artıq maksimum sayda ({MAX_FILES_PER_TASK}) fayl yüklənib.");
            }

            long totalExistingSize = existingAttachments.Sum(a => a.SizeInBytes);
            if (totalExistingSize + incomingFileSize > MAX_TOTAL_SIZE_PER_TASK)
            {
                throw new Exception($"Bu tapşırıq üçün ayrılmış ümumi yaddaş limiti (150 MB) aşıldı. Mövcud həcm: {totalExistingSize / 1024 / 1024} MB.");
            }

            
            var (filePath, thumbnailPath) = await _storageService.UploadFileAsync(request.File, "task_attachments");

           
            var attachment = new TaskAttachment
            {
                OriginalFileName = request.File.FileName,
                StoredFileName = Path.GetFileName(filePath),
                Extension = extension,
                MimeType = request.File.ContentType,
                SizeInBytes = incomingFileSize,
                FilePath = filePath,
                ThumbnailPath = thumbnailPath,
                ProjectTaskId = request.TaskId,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _attachmentWriteRepository.AddAsync(attachment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}