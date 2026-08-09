using System;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.DTOs.UserSettings;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    public interface IUserSettingsService
    {
        Task<UserSettingsDto> GetSettingsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<UserSettingsDto> UpdateSettingsAsync(Guid userId, UpdateUserSettingsDto dto, CancellationToken cancellationToken = default);
    }
}
