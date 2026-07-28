using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Queries.GetTasksByTeam
{
    public class GetTasksByTeamHandler
        : IRequestHandler<GetTasksByTeamRequest, GetTasksByTeamResponse>
    {
        private readonly ITaskItemService _taskItemService;

        public GetTasksByTeamHandler(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        public async Task<GetTasksByTeamResponse> Handle(
            GetTasksByTeamRequest request,
            CancellationToken cancellationToken)
        {
            var tasks = await _taskItemService.GetByTeamAsync(request.TeamId);

            return new GetTasksByTeamResponse
            {
                Succeeded = true,
                Message = "Komandanın tapşırıqları uğurla əldə edildi.",
                Tasks = tasks
            };
        }
    }
}