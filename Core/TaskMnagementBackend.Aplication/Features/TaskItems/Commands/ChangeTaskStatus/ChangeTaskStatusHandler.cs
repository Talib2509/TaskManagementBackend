using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Commands.ChangeTaskStatus
{
    public class ChangeTaskStatusHandler
        : IRequestHandler<ChangeTaskStatusRequest, ChangeTaskStatusResponse>
    {
        private readonly ITaskItemService _taskItemService;

        public ChangeTaskStatusHandler(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        public async Task<ChangeTaskStatusResponse> Handle(
            ChangeTaskStatusRequest request,
            CancellationToken cancellationToken)
        {
            var task = await _taskItemService.GetByIdAsync(request.Id);

            if (task == null)
            {
                return new ChangeTaskStatusResponse
                {
                    Succeeded = false,
                    Message = "Tapşırıq tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            var result = await _taskItemService.ChangeStatusAsync(
                request.Id,
                request.Status);

            if (!result)
            {
                return new ChangeTaskStatusResponse
                {
                    Succeeded = false,
                    Message = "Tapşırığın statusu dəyişdirilə bilmədi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new ChangeTaskStatusResponse
            {
                Succeeded = true,
                Message = "Tapşırığın statusu uğurla dəyişdirildi."
            };
        }
    }
}