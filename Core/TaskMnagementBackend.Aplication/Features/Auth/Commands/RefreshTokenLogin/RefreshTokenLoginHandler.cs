using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Aplication.Features.Auth.Commands.RefreshTokenLogin
{
    public class RefreshTokenLoginCommandHandler
     : IRequestHandler<RefreshTokenLoginRequest, RefreshTokenLoginResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public RefreshTokenLoginCommandHandler(
            UserManager<AppUser> userManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<RefreshTokenLoginResponse> Handle(
            RefreshTokenLoginRequest request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return new RefreshTokenLoginResponse
                {
                    Succeeded = false,
                    Message = "Refresh token boş ola bilməz."
                };
            }

            var user = _userManager.Users.FirstOrDefault(
                x => x.RefreshToken == request.RefreshToken);

            if (user is null)
            {
                return new RefreshTokenLoginResponse
                {
                    Succeeded = false,
                    Message = "Refresh token yanlışdır."
                };
            }

            // Prevent refresh login for deleted or deactivated accounts
            if (!user.IsActive || user.IsDeleted)
            {
                return new RefreshTokenLoginResponse
                {
                    Succeeded = false,
                    Message = "Hesab deaktiv edilib."
                };
            }

            if (user.RefreshTokenEndDate is null ||
                user.RefreshTokenEndDate <= DateTime.UtcNow)
            {
                return new RefreshTokenLoginResponse
                {
                    Succeeded = false,
                    Message = "Refresh token vaxtı bitib."
                };
            }

      

            var roles = await _userManager.GetRolesAsync(user);

            var accessTokenExpireDate = _tokenService.CreateAccessTokenExpireDate();

            var newAccessToken = await _tokenService.CreateAccessTokenAsync(
                user,
                roles,
                accessTokenExpireDate);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenEndDate = _tokenService.CreateRefreshTokenExpireDate();

            await _userManager.UpdateAsync(user);

            return new RefreshTokenLoginResponse
            {
                Succeeded = true,
                AccessToken = newAccessToken,
                AccessTokenExpiration = accessTokenExpireDate,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiration = user.RefreshTokenEndDate,
                Message = "Token yeniləndi."
            };
        }
    }
}
