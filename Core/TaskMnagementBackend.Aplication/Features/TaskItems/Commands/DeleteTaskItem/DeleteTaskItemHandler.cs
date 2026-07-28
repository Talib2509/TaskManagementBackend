using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Commands.DeleteTaskItem
{
    public class DeleteTaskItemHandler
        : IRequestHandler<DeleteTaskItemRequest, DeleteTaskItemResponse>
    {
        private readonly ITaskItemService _taskItemService;

        public DeleteTaskItemHandler(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        public async Task<DeleteTaskItemResponse> Handle(
            DeleteTaskItemRequest request,
            CancellationToken cancellationToken)
        {
            var taskItem = await _taskItemService.GetByIdAsync(request.Id);

            if (taskItem == null)
            {
                return new DeleteTaskItemResponse
                {
                    Succeeded = false,
                    Message = "Tapşırıq tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            var result = await _taskItemService.DeleteAsync(request.Id);

            if (!result)
            {
                return new DeleteTaskItemResponse
                {
                    Succeeded = false,
                    Message = "Tapşırıq silinərkən xəta baş verdi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new DeleteTaskItemResponse
            {
                Succeeded = true,
                Message = "Tapşırıq uğurla silindi."
            };
        }
    }
}