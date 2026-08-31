
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.DTOs.Team;
using TaskMnagementBackend.Aplication.IUnitOfWork;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TeamService(IUnitOfWork unitOfWork, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IQueryable<Team> GetAll()
        {
            return _unitOfWork.TeamReadRepository
                .GetAll()
                .Where(x => !x.IsDeleted);
        }

        public async Task<Team?> GetByIdAsync(int id)
        {
            var team = await _unitOfWork.TeamReadRepository.GetByIdAsync(id);

            if (team == null || team.IsDeleted)
                return null;

            return team;
        }

        public async Task<Team?> GetByLeadIdAsync(Guid leadId)
        {
            return await _unitOfWork.TeamReadRepository
                .GetSingleAsync(x => x.TeamLeadId == leadId && !x.IsDeleted);
        }

        public async Task<bool> CreateAsync(CreateTeamDto dto)
        {
            var company = await _unitOfWork.CompanyReadRepository
                .GetByIdAsync(dto.CompanyId);

            if (company == null || company.IsDeleted)
                return false;

            var entity = new Team
            {
                Name = dto.Name,
                Description = dto.Description,
                CompanyId = dto.CompanyId,
                TeamLeadId = dto.TeamLeadId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.TeamWriteRepository.AddAsync(entity);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(UpdateTeamDto dto)
        {
            var entity = await _unitOfWork.TeamReadRepository
                .GetByIdAsync(dto.Id);

            if (entity == null || entity.IsDeleted)
                return false;

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.CompanyId = dto.CompanyId;
            entity.TeamLeadId = dto.TeamLeadId;

            _unitOfWork.TeamWriteRepository.Update(entity);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.TeamReadRepository
                .GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return false;

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;

            _unitOfWork.TeamWriteRepository.Update(entity);

            var res = await _unitOfWork.SaveChangesAsync() > 0;

            if (res)
            {
                try
                {
                    var user = _httpContextAccessor.HttpContext?.User;
                    var userIdStr = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user?.FindFirst("UserId")?.Value;
                    Guid.TryParse(userIdStr, out var uid);

                    await _auditLogService.LogAsync(
                        action: "TeamDeleted",
                        entityType: "Team",
                        entityId: entity.Id.ToString(),
                        details: $"Team '{entity.Name}' ({entity.Id}) was soft-deleted.",
                        userId: uid,
                        userEmail: user?.FindFirst(ClaimTypes.Email)?.Value,
                        userName: user?.Identity?.Name,
                        ipAddress: _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
                }
                catch { }
            }

            return res;
        }

        public async Task<bool> AssignLeadAsync(int teamId, Guid userId)
        {
            var team = await _unitOfWork.TeamReadRepository
                .GetByIdAsync(teamId);

            if (team == null || team.IsDeleted)
                return false;

            var user = await _unitOfWork.UserManager
                .FindByIdAsync(userId.ToString());

            if (user == null)
                return false;

            team.TeamLeadId = userId;

            _unitOfWork.TeamWriteRepository.Update(team);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public Task<IEnumerable<TeamDto>> GetMyTeamsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<TeamStatisticsDto> GetStatisticsAsync(int teamId)
        {
            throw new NotImplementedException();
        }
    }
}