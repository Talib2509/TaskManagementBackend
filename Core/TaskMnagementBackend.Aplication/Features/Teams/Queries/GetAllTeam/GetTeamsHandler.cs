using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.Common.Pagination;
using TaskMnagementBackend.Aplication.DTOs.Team;

namespace TaskMnagementBackend.Aplication.Features.Teams.Queries.GetAllTeam
{
    public class GetAllTeamHandler
        : IRequestHandler<GetAllTeamRequest, GetAllTeamResponse>
    {
        private readonly ITeamService _teamService;

        public GetAllTeamHandler(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public async Task<GetAllTeamResponse> Handle(
            GetAllTeamRequest request,
            CancellationToken cancellationToken)
        {
            var query = _teamService.GetAll();

            var sortMap = new Dictionary<string, Expression<Func<Domain.Entities.Team, object>>>
            {
                ["name"] = x => x.Name,
                ["createdat"] = x => x.CreatedAt
            };

            query = query.ApplySort(
                request.SortBy,
                request.Desc,
                sortMap,
                "createdat");

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new TeamDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CompanyId = x.CompanyId,
                    TeamLeadId = x.TeamLeadId,
                    CreatedAt = x.CreatedAt,
                    IsDeleted = x.IsDeleted,
                    DeletedAt = x.DeletedAt
                })
                .ToListAsync(cancellationToken);

            return new GetAllTeamResponse
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}