using Microsoft.EntityFrameworkCore;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.DTOs.TeamInvitation;
using TaskMnagementBackend.Aplication.IUnitOfWork;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class TeamInvitationService : ITeamInvitationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeamInvitationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateAsync(CreateTeamInvitationDto dto)
        {
            var team = await _unitOfWork.TeamReadRepository
                .GetByIdAsync(dto.TeamId);

            if (team == null)
                return false;

            var exists = await _unitOfWork.TeamInvitationReadRepository
                .GetSingleAsync(x =>
                    x.TeamId == dto.TeamId &&
                    x.Email.ToLower() == dto.Email.ToLower() &&
                    x.Status == InvitationStatus.Pending);

            if (exists != null)
                return false;

            var user = await _unitOfWork.UserManager
                .FindByEmailAsync(dto.Email);

            var invitation = new TeamInvitation
            {
                TeamId = dto.TeamId,
                Email = dto.Email,
                InvitedUserId = user?.Id,
                InvitedByUserId = dto.InvitedByUserId,
                Token = Guid.NewGuid().ToString("N"),
                Status = InvitationStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _unitOfWork.TeamInvitationWriteRepository
                .AddAsync(invitation);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<bool> AcceptAsync(int invitationId)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var invitation = await _unitOfWork.TeamInvitationReadRepository
                    .GetByIdAsync(invitationId);

                if (invitation == null)
                    return false;

                if (invitation.Status != InvitationStatus.Pending)
                    return false;

                if (invitation.ExpiresAt <= DateTime.UtcNow)
                    return false;

                if (invitation.InvitedUserId == null)
                    return false;

                var member = await _unitOfWork.TeamMemberReadRepository
                    .GetSingleAsync(x =>
                        x.TeamId == invitation.TeamId &&
                        x.UserId == invitation.InvitedUserId.Value);

                if (member != null)
                    return false;

                await _unitOfWork.TeamMemberWriteRepository.AddAsync(new TeamMember
                {
                    TeamId = invitation.TeamId,
                    UserId = invitation.InvitedUserId.Value,
                    Role = TeamMemberRole.Member,
                    JoinedAt = DateTime.UtcNow
                });

                invitation.Status = InvitationStatus.Accepted;
                invitation.RespondedAt = DateTime.UtcNow;

                _unitOfWork.TeamInvitationWriteRepository.Update(invitation);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> RejectAsync(int invitationId)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var invitation = await _unitOfWork.TeamInvitationReadRepository
                    .GetByIdAsync(invitationId);

                if (invitation == null)
                    return false;

                if (invitation.Status != InvitationStatus.Pending)
                    return false;

                if (invitation.ExpiresAt <= DateTime.UtcNow)
                    return false;

                invitation.Status = InvitationStatus.Rejected;
                invitation.RespondedAt = DateTime.UtcNow;

                _unitOfWork.TeamInvitationWriteRepository.Update(invitation);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task<int> ExpireInvitationsAsync()
        {
            var invitations = await _unitOfWork.TeamInvitationReadRepository
                .GetWhere(x =>
                    x.Status == InvitationStatus.Pending &&
                    x.ExpiresAt <= DateTime.UtcNow)
                .ToListAsync();

            if (!invitations.Any())
                return 0;

            foreach (var invitation in invitations)
            {
                invitation.Status = InvitationStatus.Expired;
                invitation.RespondedAt = DateTime.UtcNow;

                _unitOfWork.TeamInvitationWriteRepository
                    .Update(invitation);
            }

            return await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var invitation = await _unitOfWork.TeamInvitationReadRepository
                .GetByIdAsync(id);

            if (invitation == null)
                return false;

            _unitOfWork.TeamInvitationWriteRepository
                .Delete(invitation);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public IQueryable<TeamInvitationDto> GetAll()
        {
            return _unitOfWork.TeamInvitationReadRepository
                .GetAll()
                .Select(x => new TeamInvitationDto
                {
                    Id = x.Id,
                    TeamId = x.TeamId,
                    Email = x.Email,
                    InvitedByUserId = x.InvitedByUserId,
                    InvitedUserId = x.InvitedUserId,
                    Token = x.Token,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    ExpiresAt = x.ExpiresAt,
                    RespondedAt = x.RespondedAt
                });
        }

        public async Task<TeamInvitationDto?> GetByIdAsync(int id)
        {
            var invitation = await _unitOfWork.TeamInvitationReadRepository
                .GetByIdAsync(id);

            if (invitation == null)
                return null;

            return new TeamInvitationDto
            {
                Id = invitation.Id,
                TeamId = invitation.TeamId,
                Email = invitation.Email,
                InvitedByUserId = invitation.InvitedByUserId,
                InvitedUserId = invitation.InvitedUserId,
                Token = invitation.Token,
                Status = invitation.Status,
                CreatedAt = invitation.CreatedAt,
                ExpiresAt = invitation.ExpiresAt,
                RespondedAt = invitation.RespondedAt
            };
        }

        public async Task<TeamInvitationDto?> GetByTokenAsync(string token)
        {
            var invitation = await _unitOfWork.TeamInvitationReadRepository
                .GetSingleAsync(x => x.Token == token);

            if (invitation == null)
                return null;

            return new TeamInvitationDto
            {
                Id = invitation.Id,
                TeamId = invitation.TeamId,
                Email = invitation.Email,
                InvitedByUserId = invitation.InvitedByUserId,
                InvitedUserId = invitation.InvitedUserId,
                Token = invitation.Token,
                Status = invitation.Status,
                CreatedAt = invitation.CreatedAt,
                ExpiresAt = invitation.ExpiresAt,
                RespondedAt = invitation.RespondedAt
            };
        }

        public async Task<IEnumerable<TeamInvitationDto>> GetPendingInvitationsAsync(Guid userId)
        {
            return await _unitOfWork.TeamInvitationReadRepository
                .GetWhere(x =>
                    x.InvitedUserId == userId &&
                    x.Status == InvitationStatus.Pending &&
                    x.ExpiresAt > DateTime.UtcNow)
                .Select(x => new TeamInvitationDto
                {
                    Id = x.Id,
                    TeamId = x.TeamId,
                    Email = x.Email,
                    InvitedByUserId = x.InvitedByUserId,
                    InvitedUserId = x.InvitedUserId,
                    Token = x.Token,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    ExpiresAt = x.ExpiresAt,
                    RespondedAt = x.RespondedAt
                })
                .ToListAsync();
        }

    }
}