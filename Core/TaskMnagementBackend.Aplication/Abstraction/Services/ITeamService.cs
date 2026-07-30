using TaskMnagementBackend.Aplication.DTOs.Team;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    public interface ITeamService
    {
        IQueryable<Team> GetAll();

        Task<Team?> GetByIdAsync(int id);

        Task<Team?> GetByLeadIdAsync(Guid leadId);

        Task<IEnumerable<TeamDto>> GetMyTeamsAsync(Guid userId);

        Task<TeamStatisticsDto> GetStatisticsAsync(int teamId);

        Task<bool> CreateAsync(CreateTeamDto dto);

        Task<bool> UpdateAsync(UpdateTeamDto dto);

        Task<bool> DeleteAsync(int id);

        Task<bool> AssignLeadAsync(int teamId, Guid userId);
    }
}