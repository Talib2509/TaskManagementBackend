using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.TeamMembers.Queries.GetTeamMemberById
{
    public class GetTeamMemberByIdHandler
        : IRequestHandler<GetTeamMemberByIdRequest, GetTeamMemberByIdResponse>
    {
        private readonly ITeamMemberService _teamMemberService;

        public GetTeamMemberByIdHandler(ITeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }

        public async Task<GetTeamMemberByIdResponse> Handle(
            GetTeamMemberByIdRequest request,
            CancellationToken cancellationToken)
        {
            var member = await _teamMemberService.GetByIdAsync(request.Id);

            if (member == null)
            {
                return new GetTeamMemberByIdResponse
                {
                    Succeeded = false,
                    Message = "Komanda üzvü tapılmadı.",
                    ErrorType = ResultErrorType.NotFound
                };
            }

            return new GetTeamMemberByIdResponse
            {
                Succeeded = true,
                Message = "Komanda üzvü uğurla əldə edildi.",
                TeamMember = member
            };
        }
    }
}