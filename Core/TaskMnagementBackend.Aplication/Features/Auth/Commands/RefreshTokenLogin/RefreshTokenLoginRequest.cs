using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Auth.Commands.RefreshTokenLogin
{
    public class RefreshTokenLoginRequest : IRequest<RefreshTokenLoginResponse>
    {
        public string RefreshToken { get; set; }
    }
}
