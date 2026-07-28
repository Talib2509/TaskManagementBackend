using MediatR;
using System.Linq.Expressions;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common.Pagination;
using TaskMnagementBackend.Aplication.DTOs.TeamInvitation;

namespace TaskMnagementBackend.Aplication.Features.TeamInvitations.Queries.GetAllTeamInvitation
{
    public class GetAllTeamInvitationHandler
        : IRequestHandler<GetAllTeamInvitationRequest, GetAllTeamInvitationResponse>
    {
        private readonly ITeamInvitationService _teamInvitationService;

        public GetAllTeamInvitationHandler(ITeamInvitationService teamInvitationService)
        {
            _teamInvitationService = teamInvitationService;
        }

        public async Task<GetAllTeamInvitationResponse> Handle(
            GetAllTeamInvitationRequest request,
            CancellationToken cancellationToken)
        {
            var query = _teamInvitationService.GetAll();

            var sortMap = new Dictionary<string, Expression<Func<TeamInvitationDto, object>>>
            {
                ["email"] = x => x.Email,
                ["createdat"] = x => x.CreatedAt,
                ["expiresat"] = x => x.ExpiresAt,
                ["status"] = x => x.Status
            };

            query = query.ApplySort(
                request.SortBy,
                request.Desc,
                sortMap,
                "createdat");

            var result = await query.ToPagedResultAsync(
                request.Page,
                request.PageSize,
                cancellationToken);

            return new GetAllTeamInvitationResponse
            {
                Items = result.Items,
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }
    }
}