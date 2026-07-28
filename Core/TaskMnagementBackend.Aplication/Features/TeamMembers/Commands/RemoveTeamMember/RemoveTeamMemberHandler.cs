using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.TeamMembers.Commands.RemoveTeamMember
{
    public class RemoveTeamMemberHandler
        : IRequestHandler<RemoveTeamMemberRequest, RemoveTeamMemberResponse>
    {
        private readonly ITeamMemberService _teamMemberService;

        public RemoveTeamMemberHandler(ITeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }

        public async Task<RemoveTeamMemberResponse> Handle(
            RemoveTeamMemberRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _teamMemberService.RemoveMemberAsync(
                request.TeamId,
                request.UserId);

            if (!result.Succeeded)
            {
                return new RemoveTeamMemberResponse
                {
                    Succeeded = false,
                    Message = result.Message,
                    ErrorType = ResultErrorType.Error,
                    Result = result
                };
            }

            return new RemoveTeamMemberResponse
            {
                Succeeded = true,
                Message = result.Message,
                Result = result
            };
        }
    }
}