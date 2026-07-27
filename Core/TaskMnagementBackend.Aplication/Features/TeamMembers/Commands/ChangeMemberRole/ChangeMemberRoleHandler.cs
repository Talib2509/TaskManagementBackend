using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.TeamMembers.Commands.ChangeMemberRole
{
    public class ChangeMemberRoleHandler
        : IRequestHandler<ChangeMemberRoleRequest, ChangeMemberRoleResponse>
    {
        private readonly ITeamMemberService _teamMemberService;

        public ChangeMemberRoleHandler(ITeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }

        public async Task<ChangeMemberRoleResponse> Handle(
            ChangeMemberRoleRequest request,
            CancellationToken cancellationToken)
        {
            var member = await _teamMemberService.GetByUserAsync(
                request.TeamId,
                request.UserId);

            if (member == null)
            {
                return new ChangeMemberRoleResponse
                {
                    Succeeded = false,
                    Message = "Komanda üzvü tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            var result = await _teamMemberService.ChangeRoleAsync(
                request.TeamId,
                request.UserId,
                request.Role);

            if (!result)
            {
                return new ChangeMemberRoleResponse
                {
                    Succeeded = false,
                    Message = "Üzvün rolu dəyişdirilə bilmədi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new ChangeMemberRoleResponse
            {
                Succeeded = true,
                Message = "Üzvün rolu uğurla dəyişdirildi."
            };
        }
    }
}