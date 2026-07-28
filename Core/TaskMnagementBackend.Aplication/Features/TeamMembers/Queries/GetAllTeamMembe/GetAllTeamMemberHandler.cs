using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common.Pagination;
using TaskMnagementBackend.Aplication.DTOs.TeamMember;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Aplication.Features.TeamMembers.Queries.GetAllTeamMember
{
    public class GetAllTeamMemberHandler
        : IRequestHandler<GetAllTeamMemberRequest, GetAllTeamMemberResponse>
    {
        private readonly ITeamMemberService _teamMemberService;

        public GetAllTeamMemberHandler(ITeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }

        public async Task<GetAllTeamMemberResponse> Handle(
            GetAllTeamMemberRequest request,
            CancellationToken cancellationToken)
        {
            var query = _teamMemberService.GetAll();

            var sortMap = new Dictionary<string, Expression<Func<TeamMemberDto, object>>>
            {
                ["joinedat"] = x => x.JoinedAt,
                ["role"] = x => x.Role,
                ["teamid"] = x => x.TeamId
            };

            query = query.ApplySort(
                request.SortBy,
                request.Desc,
                sortMap,
                "joinedat");

            var pagedResult = await query.ToPagedResultAsync(
                request.Page,
                request.PageSize,
                cancellationToken);

            return new GetAllTeamMemberResponse
            {
                Items = pagedResult.Items,
                TotalCount = pagedResult.TotalCount,
                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize
            };
        }
    }
}