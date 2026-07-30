using TaskMnagementBackend.Aplication.DTOs.TeamMember;
using TaskMnagementBackend.Domain.Enums;

public interface ITeamMemberService
{
    IQueryable<TeamMemberDto> GetAll();

    Task<TeamMemberDto?> GetByIdAsync(int id);

    Task<IEnumerable<TeamMemberDto>> GetByTeamIdAsync(int teamId);

    Task<TeamMemberDto?> GetByUserAsync(int teamId, Guid userId);

    Task<bool> CreateAsync(CreateTeamMemberDto dto);

    Task<RemoveTeamMemberResultDto> RemoveMemberAsync(int teamId, Guid userId);

    Task<bool> ChangeRoleAsync(int teamId, Guid userId, TeamMemberRole role);

    Task<bool> ExistsAsync(int teamId, Guid userId);
}