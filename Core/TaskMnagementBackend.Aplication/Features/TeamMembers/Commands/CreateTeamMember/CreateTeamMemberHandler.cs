using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;
using TaskMnagementBackend.Aplication.DTOs.TeamMember;

namespace TaskMnagementBackend.Aplication.Features.TeamMembers.Commands.CreateTeamMember
{
    public class CreateTeamMemberHandler
        : IRequestHandler<CreateTeamMemberRequest, CreateTeamMemberResponse>
    {
        private readonly ITeamMemberService _teamMemberService;

        public CreateTeamMemberHandler(ITeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }

        public async Task<CreateTeamMemberResponse> Handle(
            CreateTeamMemberRequest request,
            CancellationToken cancellationToken)
        {
            var exists = await _teamMemberService.ExistsAsync(
                request.TeamId,
                request.UserId);

            if (exists)
            {
                return new CreateTeamMemberResponse
                {
                    Succeeded = false,
                    Message = "İstifadəçi artıq bu komandanın üzvüdür.",
                    ErrorType = ResultErrorType.Conflict
                };
            }

            var result = await _teamMemberService.CreateAsync(
                new CreateTeamMemberDto
                {
                    TeamId = request.TeamId,
                    UserId = request.UserId,
                    Role = request.Role
                });

            if (!result)
            {
                return new CreateTeamMemberResponse
                {
                    Succeeded = false,
                    Message = "Komanda üzvü yaradılarkən xəta baş verdi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new CreateTeamMemberResponse
            {
                Succeeded = true,
                Message = "Komanda üzvü uğurla yaradıldı."
            };
        }
    }
}