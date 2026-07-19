using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Aplication.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailHandler
      : IRequestHandler<ConfirmEmailRequest, ConfirmEmailResponse>
    {
        private readonly UserManager<AppUser> _userManager;

        public ConfirmEmailHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ConfirmEmailResponse> Handle(
            ConfirmEmailRequest request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user is null)
            {
                return new ConfirmEmailResponse
                {
                    Succeeded = false,
                    Message = "İstifadəçi tapılmadı."
                };
            }

            if (user.EmailConfirmed)
            {
                return new ConfirmEmailResponse
                {
                    Succeeded = true,
                    Message = "Email artıq təsdiqlənib."
                };
            }

            var token = Uri.UnescapeDataString(request.Token.Replace(" ", "+"));

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return new ConfirmEmailResponse
                {
                    Succeeded = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            return new ConfirmEmailResponse
            {
                Succeeded = true,
                Message = "Email uğurla təsdiqləndi."
            };
        }
    }
}
