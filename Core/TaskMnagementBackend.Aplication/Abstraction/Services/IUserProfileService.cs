using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.DTOs.UserProfile;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    public interface IUserProfileService
    {
        Task<UserProfileDto?> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateUserProfileDto dto, CancellationToken cancellationToken = default);
        Task<string> UploadAvatarAsync(Guid userId, IFormFile file, CancellationToken cancellationToken = default);
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeactivateAccountAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
