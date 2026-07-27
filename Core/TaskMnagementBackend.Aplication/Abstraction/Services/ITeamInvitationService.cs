using System.Linq;
using TaskMnagementBackend.Aplication.DTOs.TeamInvitation;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    public interface ITeamInvitationService
    {
        IQueryable<TeamInvitationDto> GetAll();

        Task<TeamInvitationDto?> GetByIdAsync(int id);

        Task<IEnumerable<TeamInvitationDto>> GetPendingInvitationsAsync(Guid userId);

        Task<TeamInvitationDto?> GetByTokenAsync(string token);

        Task<bool> CreateAsync(CreateTeamInvitationDto dto);

        Task<bool> AcceptAsync(int invitationId);

        Task<bool> RejectAsync(int invitationId);

        Task<bool> DeleteAsync(int id);

        Task<int> ExpireInvitationsAsync();
    }
}