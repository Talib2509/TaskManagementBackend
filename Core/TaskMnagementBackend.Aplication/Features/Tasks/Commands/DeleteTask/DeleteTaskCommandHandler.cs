using MediatR;
using Microsoft.AspNetCore.Http;
using SmartTask.Domain.Entities;
using System.Security.Claims;
using TaskMnagementBackend.Aplication.Abstraction.IRepositories;

namespace TaskMnagementBackend.Aplication.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommandRequest, DeleteTaskCommandResponse>
    {
        private readonly IReadRepository<ProjectTask> _taskReadRepository;
        private readonly IWriteRepository<ProjectTask> _taskWriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteTaskCommandHandler(
            IReadRepository<ProjectTask> taskReadRepository,
            IWriteRepository<ProjectTask> taskWriteRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _taskReadRepository = taskReadRepository;
            _taskWriteRepository = taskWriteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DeleteTaskCommandResponse> Handle(
            DeleteTaskCommandRequest request,
            CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return new DeleteTaskCommandResponse { Success = false, Message = "İstifadəçi tapılmadı." };
            }

            var task = await _taskReadRepository.GetSingleAsync(x => x.Id == request.Id && x.UserId == userId);

            if (task is null)
            {
                return new DeleteTaskCommandResponse { Success = false, Message = "Tapşırıq tapılmadı və ya silməyə icazəniz yoxdur." };
            }

            _taskWriteRepository.Delete(task);
            await _taskWriteRepository.SaveAsync();

            return new DeleteTaskCommandResponse
            {
                Success = true,
                Message = "Tapşırıq uğurla silindi."
            };
        }
    }
}
