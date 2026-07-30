using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.TaskItem;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Commands.UpdateTaskItem
{
    public class UpdateTaskItemHandler
        : IRequestHandler<UpdateTaskItemRequest, UpdateTaskItemResponse>
    {
        private readonly ITaskItemService _taskItemService;

        public UpdateTaskItemHandler(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        public async Task<UpdateTaskItemResponse> Handle(
            UpdateTaskItemRequest request,
            CancellationToken cancellationToken)
        {
            var task = await _taskItemService.GetByIdAsync(request.Id);

            if (task == null)
            {
                return new UpdateTaskItemResponse
                {
                    Succeeded = false,
                    Message = "Tapşırıq tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            var result = await _taskItemService.UpdateAsync(
                new UpdateTaskItemDto
                {
                    Id = request.Id,
                    Title = request.Title,
                    TeamId = request.TeamId,
                    AssignedUserId = request.AssignedUserId,
                    IsPrivate = request.IsPrivate,
                    Status = request.Status
                });

            if (!result)
            {
                return new UpdateTaskItemResponse
                {
                    Succeeded = false,
                    Message = "Tapşırıq yenilənərkən xəta baş verdi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new UpdateTaskItemResponse
            {
                Succeeded = true,
                Message = "Tapşırıq uğurla yeniləndi."
            };
        }
    }
}