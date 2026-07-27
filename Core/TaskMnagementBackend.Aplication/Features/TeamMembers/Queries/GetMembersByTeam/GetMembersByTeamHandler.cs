using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.TeamMembers.Queries.GetMembersByTeam
{
    public class GetMembersByTeamHandler
        : IRequestHandler<GetMembersByTeamRequest, GetMembersByTeamResponse>
    {
        private readonly ITeamMemberService _teamMemberService;

        public GetMembersByTeamHandler(ITeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }

        public async Task<GetMembersByTeamResponse> Handle(
            GetMembersByTeamRequest request,
            CancellationToken cancellationToken)
        {
            var members = await _teamMemberService.GetByTeamIdAsync(request.TeamId);

            if (members == null || !members.Any())
            {
                return new GetMembersByTeamResponse
                {
                    Succeeded = false,
                    Message = "Bu komandaya aid üzv tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            return new GetMembersByTeamResponse
            {
                Succeeded = true,
                Message = "Komanda üzvləri uğurla əldə edildi.",
                Members = members
            };
        }
    }
}