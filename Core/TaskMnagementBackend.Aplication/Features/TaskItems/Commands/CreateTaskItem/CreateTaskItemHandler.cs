using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.TaskItem;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Commands.CreateTaskItem
{
    public class CreateTaskItemHandler
        : IRequestHandler<CreateTaskItemRequest, CreateTaskItemResponse>
    {
        private readonly ITaskItemService _taskItemService;

        public CreateTaskItemHandler(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        public async Task<CreateTaskItemResponse> Handle(
            CreateTaskItemRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _taskItemService.CreateAsync(
                new CreateTaskItemDto
                {
                    Title = request.Title,
                    Description = request.Description,
                    TeamId = request.TeamId,
                    AssignedUserId = request.AssignedUserId,
                    DueDate = request.DueDate,
                    Priority = request.Priority
                });

            if (!result)
            {
                return new CreateTaskItemResponse
                {
                    Succeeded = false,
                    Message = "Tapşırıq yaradılarkən xəta baş verdi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new CreateTaskItemResponse
            {
                Succeeded = true,
                Message = "Tapşırıq uğurla yaradıldı."
            };
        }
    }
}