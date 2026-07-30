using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;
using TaskMnagementBackend.Aplication.Features.Commands.DeleteTaskAttachment;
using TaskMnagementBackend.Aplication.Features.Commands.UploadTaskAttachment;
using TaskMnagementBackend.Aplication.Features.Queries.GetTaskAttachments;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Api.Controllers
{
  
    public class UploadFileDto
    {
        public IFormFile File { get; set; }
    }

    public class UploadChunkDto
    {
        public IFormFile Chunk { get; set; }
        public string FileGuid { get; set; }
        public string FileName { get; set; }
        public int ChunkIndex { get; set; }
        public int TotalChunks { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttachmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IReadRepository<TaskAttachment> _attachmentReadRepository;

        public AttachmentsController(IMediator mediator, IReadRepository<TaskAttachment> attachmentReadRepository)
        {
            _mediator = mediator;
            _attachmentReadRepository = attachmentReadRepository;
        }

        
        [HttpPost("task/{taskId}")]
        public async Task<IActionResult> Upload([FromRoute] int taskId, [FromForm] UploadFileDto dto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId)) return Unauthorized();

            var command = new UploadTaskAttachmentCommand
            {
                TaskId = taskId,
                UserId = userId,
                File = dto.File
            };

            await _mediator.Send(command);
            return Ok(new { message = "Fayl uğurla yükləndi." });
        }

   
        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetTaskAttachments([FromRoute] int taskId)
        {
            var result = await _mediator.Send(new GetTaskAttachmentsQuery { TaskId = taskId });
            return Ok(result);
        }

  
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttachment([FromRoute] int id)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId)) return Unauthorized();

            await _mediator.Send(new DeleteTaskAttachmentCommand { AttachmentId = id, UserId = userId });
            return Ok(new { message = "Fayl uğurla silindi." });
        }

       
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile([FromRoute] int id)
        {
            var attachment = await _attachmentReadRepository.GetByIdAsync(id);
            if (attachment == null || attachment.IsDeleted) return NotFound("Fayl tapılmadı.");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", attachment.FilePath);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Fayl diskdə tapılmadı.");

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(stream, attachment.MimeType, attachment.OriginalFileName);
        }

       
        [HttpPost("task/{taskId}/chunk")]
        public async Task<IActionResult> UploadChunk([FromRoute] int taskId, [FromForm] UploadChunkDto dto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out Guid userId)) return Unauthorized();

            var command = new UploadChunkCommand
            {
                TaskId = taskId,
                UserId = userId,
                Chunk = dto.Chunk,
                FileGuid = dto.FileGuid,
                FileName = dto.FileName,
                ChunkIndex = dto.ChunkIndex,
                TotalChunks = dto.TotalChunks
            };

            await _mediator.Send(command);

            if (dto.ChunkIndex == dto.TotalChunks - 1)
            {
                return Ok(new { message = "Fayl tamamilə yükləndi və qeydə alındı." });
            }

            return Ok(new { message = $"Hissə {dto.ChunkIndex + 1}/{dto.TotalChunks} uğurla yükləndi." });
        }
    }
}