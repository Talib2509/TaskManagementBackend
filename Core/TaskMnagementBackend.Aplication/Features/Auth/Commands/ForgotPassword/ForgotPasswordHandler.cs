using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Aplication.Features.Auth.Commands.ForgotPassword
{

    public class ForgotPasswordHandler
        : IRequestHandler<ForgotPasswordRequest, ForgotPasswordResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ForgotPasswordHandler(
            UserManager<AppUser> userManager,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<ForgotPasswordResponse> Handle(
            ForgotPasswordRequest request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return new ForgotPasswordResponse
                {
                    Succeeded = false,
                    Message = "Email boş ola bilməz."
                };
            }

            var user = await _userManager.FindByEmailAsync(request.Email);

            // Security üçün burada user tapılmasa belə eyni cavab qaytarırıq.
            // Beləliklə, kənardan bu email sistemdə var/yox bilinmir.
            if (user is null || user.IsDeleted)
            {
                return new ForgotPasswordResponse
                {
                    Succeeded = true,
                    Message = "Əgər bu email sistemdə varsa, şifrə sıfırlama linki göndərildi."
                };
            }



            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);

            var frontendBaseUrl =
                _configuration["AppSettings:FrontendBaseUrl"] ?? "http://localhost:5173";

            var resetLink =
                $"{frontendBaseUrl.TrimEnd('/')}/reset-password?email={Uri.EscapeDataString(user.Email!)}&token={encodedToken}";

            var htmlBody = $@"
            <h2>CampusConnect Şifrə Sıfırlama</h2>

            <p>Salam, {user.FullName}</p>

            <p>Şifrənizi yeniləmək üçün aşağıdakı linkə klik edin:</p>

            <p>
                <a href='{resetLink}' target='_blank'>
                    Şifrəni yenilə
                </a>
            </p>

            <p>
                Əgər bu əməliyyatı siz etməmisinizsə, bu emaili nəzərə almayın.
            </p>
        ";

            await _emailService.SendEmailAsync(
                user.Email!,
                "CampusConnect - Şifrə sıfırlama",
                htmlBody);

            return new ForgotPasswordResponse
            {
                Succeeded = true,
                Message = "Əgər bu email sistemdə varsa, şifrə sıfırlama linki göndərildi."
            };
        }
    }
}
