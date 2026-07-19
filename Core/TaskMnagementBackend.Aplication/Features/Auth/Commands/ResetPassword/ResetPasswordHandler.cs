using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Aplication.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordRequest, ResetPasswordResponse>
    {
        private readonly UserManager<AppUser> _userManager;

        public ResetPasswordCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResetPasswordResponse> Handle(
            ResetPasswordRequest request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Token) ||
                string.IsNullOrWhiteSpace(request.NewPassword) ||
                string.IsNullOrWhiteSpace(request.ConfirmPassword))
            {
                return new ResetPasswordResponse
                {
                    Succeeded = false,
                    Message = "Bütün xanalar doldurulmalıdır."
                };
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return new ResetPasswordResponse
                {
                    Succeeded = false,
                    Message = "Şifrələr uyğun gəlmir."
                };
            }

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || user.IsDeleted)
            {
                return new ResetPasswordResponse
                {
                    Succeeded = false,
                    Message = "İstifadəçi tapılmadı."
                };
            }

         

            var token = Uri.UnescapeDataString(request.Token.Replace(" ", "+"));

            var result = await _userManager.ResetPasswordAsync(
                user,
                token,
                request.NewPassword);

            if (!result.Succeeded)
            {
                return new ResetPasswordResponse
                {
                    Succeeded = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            user.RefreshToken = null;
            user.RefreshTokenEndDate = null;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return new ResetPasswordResponse
                {
                    Succeeded = false,
                    Message = string.Join(", ", updateResult.Errors.Select(e => e.Description))
                };
            }

            return new ResetPasswordResponse
            {
                Succeeded = true,
                Message = "Şifrə uğurla yeniləndi."
            };
        }
    }
}
