using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Domain.Entities.Identity;

namespace TaskMnagementBackend.Aplication.Features.Auth.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public LoginHandler(
            UserManager<AppUser> userManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> Handle(
            LoginRequest request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || user.IsDeleted)
            {
                return new LoginResponse
                {
                    Succeeded = false,
                    Message = "Email və ya şifrə yanlışdır."
                };
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(
                user,
                request.Password);

            if (!isPasswordValid)
            {
                return new LoginResponse
                {
                    Succeeded = false,
                    Message = "Email və ya şifrə yanlışdır."
                };
            }

            if (!user.EmailConfirmed)
            {
                return new LoginResponse
                {
                    Succeeded = false,
                    Message = "Zəhmət olmasa əvvəlcə emailinizi təsdiqləyin."
                };
            }

            // Prevent login for deactivated or deleted users
            if (!user.IsActive || user.IsDeleted)
            {
                return new LoginResponse
                {
                    Succeeded = false,
                    Message = "Hesab deaktiv edilib."
                };
            }

        

            var roles = await _userManager.GetRolesAsync(user);

            var accessTokenExpireDate = _tokenService.CreateAccessTokenExpireDate();

            var accessToken = await _tokenService.CreateAccessTokenAsync(
                user,
                roles,
                accessTokenExpireDate);

            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenEndDate = _tokenService.CreateRefreshTokenExpireDate();

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return new LoginResponse
                {
                    Succeeded = false,
                    Message = string.Join(", ", updateResult.Errors.Select(e => e.Description))
                };
            }

            return new LoginResponse
            {
                Succeeded = true,
                Token = accessToken,
                ExpireDate = accessTokenExpireDate,
                RefreshToken = refreshToken,
                Message = "Giriş uğurludur."
            };
        }
    }
}
