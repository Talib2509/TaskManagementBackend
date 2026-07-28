using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.TaskItems.Commands.AssignTaskMember
{
    public class AssignTaskMemberHandler
        : IRequestHandler<AssignTaskMemberRequest, AssignTaskMemberResponse>
    {
        private readonly ITaskItemService _taskItemService;

        public AssignTaskMemberHandler(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        public async Task<AssignTaskMemberResponse> Handle(
            AssignTaskMemberRequest request,
            CancellationToken cancellationToken)
        {
            var task = await _taskItemService.GetByIdAsync(request.TaskId);

            if (task == null)
            {
                return new AssignTaskMemberResponse
                {
                    Succeeded = false,
                    Message = "Tapşırıq tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            var result = await _taskItemService.AssignMemberAsync(
                request.TaskId,
                request.UserId);

            if (!result)
            {
                return new AssignTaskMemberResponse
                {
                    Succeeded = false,
                    Message = "İstifadəçi tapşırığa təyin edilə bilmədi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new AssignTaskMemberResponse
            {
                Succeeded = true,
                Message = "İstifadəçi tapşırığa uğurla təyin edildi."
            };
        }
    }
}