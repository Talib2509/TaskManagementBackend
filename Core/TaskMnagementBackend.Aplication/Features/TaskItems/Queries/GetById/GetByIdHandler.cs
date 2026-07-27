using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Queries.GetTaskItemById
{
    public class GetTaskItemByIdHandler
        : IRequestHandler<GetTaskItemByIdRequest, GetTaskItemByIdResponse>
    {
        private readonly ITaskItemService _taskItemService;

        public GetTaskItemByIdHandler(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        public async Task<GetTaskItemByIdResponse> Handle(
            GetTaskItemByIdRequest request,
            CancellationToken cancellationToken)
        {
            var task = await _taskItemService.GetByIdAsync(request.Id);

            if (task == null)
            {
                return new GetTaskItemByIdResponse
                {
                    Succeeded = false,
                    Message = "Tapşırıq tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            return new GetTaskItemByIdResponse
            {
                Succeeded = true,
                Message = "Tapşırıq uğurla əldə edildi.",
                TaskItem = task
            };
        }
    }
}