using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.DTOs.TeamMember;
using TaskMnagementBackend.Aplication.IUnitOfWork;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TeamMemberService(IUnitOfWork unitOfWork, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IQueryable<TeamMemberDto> GetAll()
        {
            return _unitOfWork.TeamMemberReadRepository
                .GetAll()
                .Select(x => new TeamMemberDto
                {
                    Id = x.Id,
                    TeamId = x.TeamId,
                    UserId = x.UserId,
                    Role = x.Role,
                    JoinedAt = x.JoinedAt
                });
        }

        public async Task<TeamMemberDto?> GetByIdAsync(int id)
        {
            var member = await _unitOfWork.TeamMemberReadRepository.GetByIdAsync(id);

            if (member == null)
                return null;

            return new TeamMemberDto
            {
                Id = member.Id,
                TeamId = member.TeamId,
                UserId = member.UserId,
                Role = member.Role,
                JoinedAt = member.JoinedAt
            };
        }

        public async Task<IEnumerable<TeamMemberDto>> GetByTeamIdAsync(int teamId)
        {
            return await _unitOfWork.TeamMemberReadRepository
                .GetWhere(x => x.TeamId == teamId)
                .Select(x => new TeamMemberDto
                {
                    Id = x.Id,
                    TeamId = x.TeamId,
                    UserId = x.UserId,
                    Role = x.Role,
                    JoinedAt = x.JoinedAt
                })
                .ToListAsync();
        }

        public async Task<TeamMemberDto?> GetByUserAsync(int teamId, Guid userId)
        {
            var member = await _unitOfWork.TeamMemberReadRepository
                .GetSingleAsync(x => x.TeamId == teamId && x.UserId == userId);

            if (member == null)
                return null;

            return new TeamMemberDto
            {
                Id = member.Id,
                TeamId = member.TeamId,
                UserId = member.UserId,
                Role = member.Role,
                JoinedAt = member.JoinedAt
            };
        }

        public async Task<bool> ExistsAsync(int teamId, Guid userId)
        {
            var member = await _unitOfWork.TeamMemberReadRepository
                .GetSingleAsync(x => x.TeamId == teamId && x.UserId == userId);

            return member != null;
        }

        public async Task<bool> CreateAsync(CreateTeamMemberDto dto)
        {
            if (await ExistsAsync(dto.TeamId, dto.UserId))
                return false;

            var entity = new TeamMember
            {
                TeamId = dto.TeamId,
                UserId = dto.UserId,
                Role = dto.Role,
                JoinedAt = DateTime.UtcNow
            };

            await _unitOfWork.TeamMemberWriteRepository.AddAsync(entity);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(UpdateTeamMemberDto dto)
        {
            var member = await _unitOfWork.TeamMemberReadRepository.GetByIdAsync(dto.Id);

            if (member == null)
                return false;

            member.TeamId = dto.TeamId;
            member.UserId = dto.UserId;
            member.Role = dto.Role;

            _unitOfWork.TeamMemberWriteRepository.Update(member);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _unitOfWork.TeamMemberWriteRepository.DeleteAsync(id);

            if (!result)
                return false;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<RemoveTeamMemberResultDto> RemoveMemberAsync(int teamId, Guid userId)
        {
            var member = await _unitOfWork.TeamMemberReadRepository
                .GetSingleAsync(x => x.TeamId == teamId && x.UserId == userId);

            if (member == null)
            {
                return new RemoveTeamMemberResultDto
                {
                    Succeeded = false,
                    Message = "Komanda üzvü tapılmadı."
                };
            }

            _unitOfWork.TeamMemberWriteRepository.Delete(member);

            await _unitOfWork.SaveChangesAsync();
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;
                var userIdStr = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user?.FindFirst("UserId")?.Value;
                Guid.TryParse(userIdStr, out var uid);

                await _auditLogService.LogAsync(
                    action: "TeamMemberRemoved",
                    entityType: "TeamMember",
                    entityId: member.Id.ToString(),
                    details: $"User {member.UserId} was removed from team {member.TeamId}.",
                    userId: uid,
                    userEmail: user?.FindFirst(ClaimTypes.Email)?.Value,
                    userName: user?.Identity?.Name,
                    ipAddress: _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
            }
            catch { }

            return new RemoveTeamMemberResultDto
            {
                Succeeded = true,
                Message = "Komanda üzvü uğurla çıxarıldı.",
                TasksTransferred = false
            };
        }

        public async Task<bool> ChangeRoleAsync(int teamId, Guid userId, TeamMemberRole role)
        {
            var member = await _unitOfWork.TeamMemberReadRepository
                .GetSingleAsync(x => x.TeamId == teamId && x.UserId == userId);

            if (member == null)
                return false;

            member.Role = role;

            _unitOfWork.TeamMemberWriteRepository.Update(member);

            var res = await _unitOfWork.SaveChangesAsync() > 0;

            if (res)
            {
                try
                {
                    var user = _httpContextAccessor.HttpContext?.User;
                    var userIdStr = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user?.FindFirst("UserId")?.Value;
                    Guid.TryParse(userIdStr, out var uid);

                    await _auditLogService.LogAsync(
                        action: "TeamMemberRoleChanged",
                        entityType: "TeamMember",
                        entityId: member.Id.ToString(),
                        details: $"Team member {member.UserId} role changed to {role} in team {member.TeamId}.",
                        userId: uid,
                        userEmail: user?.FindFirst(ClaimTypes.Email)?.Value,
                        userName: user?.Identity?.Name,
                        ipAddress: _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
                }
                catch { }
            }

            return res;
        }
    }
}