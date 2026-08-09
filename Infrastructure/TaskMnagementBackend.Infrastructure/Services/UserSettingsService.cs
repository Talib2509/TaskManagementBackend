using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.DTOs.UserSettings;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly AppDbContext _dbContext;

        public UserSettingsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserSettingsDto> GetSettingsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var settings = await _dbContext.UserSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

            if (settings == null)
            {
                // Əgər hələ yaranmayıbsa, defolt tənzimləmə yaradırıq
                settings = new UserSettings
                {
                    UserId = userId,
                    EmailNotificationEnabled = true,
                    NotifyOnTaskAssigned = true,
                    NotifyOnComment = true,
                    NotifyOnStatusChange = true,
                    NotifyOnInvitation = true,
                    Language = "az",
                    Theme = "light",
                    UpdatedAt = DateTime.UtcNow
                };

                await _dbContext.UserSettings.AddAsync(settings, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return new UserSettingsDto
            {
                UserId = settings.UserId,
                EmailNotificationEnabled = settings.EmailNotificationEnabled,
                NotifyOnTaskAssigned = settings.NotifyOnTaskAssigned,
                NotifyOnComment = settings.NotifyOnComment,
                NotifyOnStatusChange = settings.NotifyOnStatusChange,
                NotifyOnInvitation = settings.NotifyOnInvitation,
                Language = settings.Language,
                Theme = settings.Theme,
                UpdatedAt = settings.UpdatedAt
            };
        }

        public async Task<UserSettingsDto> UpdateSettingsAsync(Guid userId, UpdateUserSettingsDto dto, CancellationToken cancellationToken = default)
        {
            var settings = await _dbContext.UserSettings
                .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

            if (settings == null)
            {
                settings = new UserSettings
                {
                    UserId = userId,
                    EmailNotificationEnabled = dto.EmailNotificationEnabled ?? true,
                    NotifyOnTaskAssigned = dto.NotifyOnTaskAssigned ?? true,
                    NotifyOnComment = dto.NotifyOnComment ?? true,
                    NotifyOnStatusChange = dto.NotifyOnStatusChange ?? true,
                    NotifyOnInvitation = dto.NotifyOnInvitation ?? true,
                    Language = dto.Language ?? "az",
                    Theme = dto.Theme ?? "light",
                    UpdatedAt = DateTime.UtcNow
                };

                await _dbContext.UserSettings.AddAsync(settings, cancellationToken);
            }
            else
            {
                if (dto.EmailNotificationEnabled.HasValue) settings.EmailNotificationEnabled = dto.EmailNotificationEnabled.Value;
                if (dto.NotifyOnTaskAssigned.HasValue) settings.NotifyOnTaskAssigned = dto.NotifyOnTaskAssigned.Value;
                if (dto.NotifyOnComment.HasValue) settings.NotifyOnComment = dto.NotifyOnComment.Value;
                if (dto.NotifyOnStatusChange.HasValue) settings.NotifyOnStatusChange = dto.NotifyOnStatusChange.Value;
                if (dto.NotifyOnInvitation.HasValue) settings.NotifyOnInvitation = dto.NotifyOnInvitation.Value;
                if (!string.IsNullOrWhiteSpace(dto.Language)) settings.Language = dto.Language;
                if (!string.IsNullOrWhiteSpace(dto.Theme)) settings.Theme = dto.Theme;
                settings.UpdatedAt = DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new UserSettingsDto
            {
                UserId = settings.UserId,
                EmailNotificationEnabled = settings.EmailNotificationEnabled,
                NotifyOnTaskAssigned = settings.NotifyOnTaskAssigned,
                NotifyOnComment = settings.NotifyOnComment,
                NotifyOnStatusChange = settings.NotifyOnStatusChange,
                NotifyOnInvitation = settings.NotifyOnInvitation,
                Language = settings.Language,
                Theme = settings.Theme,
                UpdatedAt = settings.UpdatedAt
            };
        }
    }
}
