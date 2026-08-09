using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.DTOs.UserProfile;
using TaskMnagementBackend.Aplication.DTOs.UserSettings;

namespace TaskMnagementBackend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IUserProfileService _profileService;
        private readonly IUserSettingsService _settingsService;

        public ProfileController(
            IUserProfileService profileService,
            IUserSettingsService settingsService)
        {
            _profileService = profileService;
            _settingsService = settingsService;
        }

        private Guid GetCurrentUserId()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("UserId")?.Value;

            if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                throw new UnauthorizedAccessException("İstifadəçi autentifikasiyadan keçməyib.");

            return userId;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetCurrentUserId();
            var profile = await _profileService.GetProfileAsync(userId);
            if (profile == null)
                return NotFound("Profil tapılmadı.");

            return Ok(profile);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserProfileDto dto)
        {
            var userId = GetCurrentUserId();
            var updated = await _profileService.UpdateProfileAsync(userId, dto);
            return Ok(updated);
        }

        [HttpPost("upload-avatar")]
        [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting("UploadPolicy")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            var userId = GetCurrentUserId();
            var avatarUrl = await _profileService.UploadAvatarAsync(userId, file);
            return Ok(new { AvatarUrl = avatarUrl, Message = "Profil şəkli uğurla yeniləndi." });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = GetCurrentUserId();
            await _profileService.ChangePasswordAsync(userId, dto);
            return Ok(new { Message = "Şifrə uğurla dəyişdirildi." });
        }

        [HttpPost("deactivate")]
        public async Task<IActionResult> DeactivateAccount()
        {
            var userId = GetCurrentUserId();
            await _profileService.DeactivateAccountAsync(userId);
            return Ok(new { Message = "Hesabınız uğurla deaktiv edildi." });
        }

        [HttpGet("settings")]
        public async Task<IActionResult> GetSettings()
        {
            var userId = GetCurrentUserId();
            var settings = await _settingsService.GetSettingsAsync(userId);
            return Ok(settings);
        }

        [HttpPut("settings")]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateUserSettingsDto dto)
        {
            var userId = GetCurrentUserId();
            var updated = await _settingsService.UpdateSettingsAsync(userId, dto);
            return Ok(updated);
        }
    }
}
