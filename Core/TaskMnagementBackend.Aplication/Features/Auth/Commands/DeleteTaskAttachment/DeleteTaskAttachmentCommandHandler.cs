using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Aplication.Features.Commands.DeleteTaskAttachment
{
    public class DeleteTaskAttachmentCommandHandler : IRequestHandler<DeleteTaskAttachmentCommand, bool>
    {
        private readonly IReadRepository<TaskAttachment> _attachmentReadRepository;
        private readonly IWriteRepository<TaskAttachment> _attachmentWriteRepository;
        private readonly IStorageService _storageService;
        private readonly TaskMnagementBackend.Aplication.IUnitOfWork.IUnitOfWork _unitOfWork;

        public DeleteTaskAttachmentCommandHandler(
            IReadRepository<TaskAttachment> attachmentReadRepository,
            IWriteRepository<TaskAttachment> attachmentWriteRepository,
            IStorageService storageService,
            TaskMnagementBackend.Aplication.IUnitOfWork.IUnitOfWork unitOfWork)
        {
            _attachmentReadRepository = attachmentReadRepository;
            _attachmentWriteRepository = attachmentWriteRepository;
            _storageService = storageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteTaskAttachmentCommand request, CancellationToken cancellationToken)
        {
            var attachment = await _attachmentReadRepository.GetByIdAsync(request.AttachmentId);

            if (attachment == null || attachment.IsDeleted)
                throw new Exception("Fayl tapılmadı!");

            
            if (attachment.UserId != request.UserId)
                throw new UnauthorizedAccessException("Siz yalnız öz yüklədiyiniz faylları silə bilərsiniz!");

            
            await _storageService.DeleteFileAsync(attachment.FilePath, "");
            if (!string.IsNullOrEmpty(attachment.ThumbnailPath))
                await _storageService.DeleteFileAsync(attachment.ThumbnailPath, "");

            
            attachment.IsDeleted = true;
            _attachmentWriteRepository.Update(attachment); 
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}