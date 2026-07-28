using MediatR;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common;

namespace TaskMnagementBackend.Aplication.Features.Teams.Commands.DeleteTeam
{
    public class DeleteTeamHandler
        : IRequestHandler<DeleteTeamRequest, DeleteTeamResponse>
    {
        private readonly ITeamService _teamService;

        public DeleteTeamHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<DeleteTeamResponse> Handle(
            DeleteTeamRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _teamService.DeleteAsync(request.TeamId);

            if (!result)
            {
                return new DeleteTeamResponse
                {
                    Succeeded = false,
                    Message = "Komanda silinərkən xəta baş verdi.",
                    ErrorType = ResultErrorType.Error
                };
            }

            return new DeleteTeamResponse
            {
                Succeeded = true,
                Message = "Komanda uğurla silindi."
            };
        }
    }
}