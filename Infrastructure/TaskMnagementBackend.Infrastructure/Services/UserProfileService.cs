using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.DTOs.UserProfile;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStorageService _storageService;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserProfileService(
            UserManager<AppUser> userManager,
            IStorageService storageService,
            IAuditLogService auditLogService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _storageService = storageService;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserProfileDto?> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            var avatarUrl = !string.IsNullOrWhiteSpace(user.ProfilePicture)
                ? _storageService.GetFileUrl(user.ProfilePicture, "avatars")
                : null;

            return new UserProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                ProfilePictureUrl = avatarUrl,
                CompanyName = user.CompanyName,
                Bio = user.Bio,
                JobTitle = user.JobTitle,
                Timezone = user.Timezone,
                IsActive = user.IsActive && !user.IsDeleted,
                ActiveTeamId = user.ActiveTeamId,
                CreatedAt = user.CreatedAt,
                Roles = roles
            };
        }

        public async Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateUserProfileDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                throw new Exception("İstifadəçi tapılmadı və ya deaktivdir.");

            if (dto.FullName != null) user.FullName = dto.FullName;
            if (dto.PhoneNumber != null) user.PhoneNumber = dto.PhoneNumber;
            if (dto.Bio != null) user.Bio = dto.Bio;
            if (dto.JobTitle != null) user.JobTitle = dto.JobTitle;
            if (dto.Timezone != null) user.Timezone = dto.Timezone;
            if (dto.ActiveTeamId.HasValue) user.ActiveTeamId = dto.ActiveTeamId.Value;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _auditLogService.LogAsync(
                action: "ProfileUpdated",
                entityType: "AppUser",
                entityId: user.Id.ToString(),
                details: $"Profil məlumatları yeniləndi. (Ad: {user.FullName}, Vəzifə: {user.JobTitle})",
                userId: user.Id,
                userEmail: user.Email,
                userName: user.UserName,
                ipAddress: GetClientIpAddress(),
                cancellationToken: cancellationToken);

            var roles = await _userManager.GetRolesAsync(user);
            var avatarUrl = !string.IsNullOrWhiteSpace(user.ProfilePicture)
                ? _storageService.GetFileUrl(user.ProfilePicture, "avatars")
                : null;

            return new UserProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture,
                ProfilePictureUrl = avatarUrl,
                CompanyName = user.CompanyName,
                Bio = user.Bio,
                JobTitle = user.JobTitle,
                Timezone = user.Timezone,
                IsActive = user.IsActive && !user.IsDeleted,
                ActiveTeamId = user.ActiveTeamId,
                CreatedAt = user.CreatedAt,
                Roles = roles
            };
        }

        public async Task<string> UploadAvatarAsync(Guid userId, IFormFile file, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                throw new Exception("İstifadəçi tapılmadı və ya deaktivdir.");

            if (file == null || file.Length == 0)
                throw new Exception("Fayl seçilməyib.");

            // Əgər köhnə şəkil varsa, silək
            if (!string.IsNullOrWhiteSpace(user.ProfilePicture))
            {
                try
                {
                    await _storageService.DeleteFileAsync(user.ProfilePicture, "avatars");
                }
                catch { }
            }

            var (filePath, thumbnailPath) = await _storageService.UploadFileAsync(file, "avatars");
            user.ProfilePicture = filePath;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                throw new Exception(string.Join(", ", updateResult.Errors.Select(e => e.Description)));

            await _auditLogService.LogAsync(
                action: "AvatarUpdated",
                entityType: "AppUser",
                entityId: user.Id.ToString(),
                details: $"Profil şəkli yeniləndi: {filePath}",
                userId: user.Id,
                userEmail: user.Email,
                userName: user.UserName,
                ipAddress: GetClientIpAddress(),
                cancellationToken: cancellationToken);

            return _storageService.GetFileUrl(filePath, "avatars");
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(dto.NewPassword) || dto.NewPassword != dto.ConfirmNewPassword)
                throw new Exception("Yeni şifrə və təkrar şifrə uyğun gəlmir.");

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                throw new Exception("İstifadəçi tapılmadı.");

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            // Refresh token-i sıfırla ki digər sessiyalar bitsin
            user.RefreshToken = null;
            user.RefreshTokenEndDate = null;
            await _userManager.UpdateAsync(user);

            await _auditLogService.LogAsync(
                action: "PasswordChanged",
                entityType: "AppUser",
                entityId: user.Id.ToString(),
                details: "İstifadəçi şifrəsini dəyişdi.",
                userId: user.Id,
                userEmail: user.Email,
                userName: user.UserName,
                ipAddress: GetClientIpAddress(),
                cancellationToken: cancellationToken);

            return true;
        }

        public async Task<bool> DeactivateAccountAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                throw new Exception("İstifadəçi tapılmadı.");

            user.IsDeleted = true;
            user.IsActive = false;
            user.RefreshToken = null;
            user.RefreshTokenEndDate = null;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _auditLogService.LogAsync(
                action: "AccountDeactivated",
                entityType: "AppUser",
                entityId: user.Id.ToString(),
                details: "İstifadəçi öz hesabını deaktiv etdi (self-deactivation).",
                userId: user.Id,
                userEmail: user.Email,
                userName: user.UserName,
                ipAddress: GetClientIpAddress(),
                cancellationToken: cancellationToken);

            return true;
        }

        private string? GetClientIpAddress()
        {
            var context = _httpContextAccessor.HttpContext;
            return context?.Connection?.RemoteIpAddress?.ToString();
        }
    }
}
