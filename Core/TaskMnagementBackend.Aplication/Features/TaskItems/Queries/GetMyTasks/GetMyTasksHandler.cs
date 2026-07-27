using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Queries.GetMyTasks
{
    public class GetMyTasksHandler
        : IRequestHandler<GetMyTasksRequest, GetMyTasksResponse>
    {
        private readonly ITaskItemService _taskItemService;

        public GetMyTasksHandler(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        public async Task<GetMyTasksResponse> Handle(
            GetMyTasksRequest request,
            CancellationToken cancellationToken)
        {
            var tasks = await _taskItemService.GetMyTasksAsync(request.UserId);

            return new GetMyTasksResponse
            {
                Succeeded = true,
                Message = "İstifadəçinin tapşırıqları uğurla əldə edildi.",
                Tasks = tasks
            };
        }
    }
}