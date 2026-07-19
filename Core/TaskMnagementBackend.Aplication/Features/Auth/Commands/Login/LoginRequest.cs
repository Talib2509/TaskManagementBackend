using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Auth.Commands.Login
{
    public class LoginRequest : IRequest<LoginResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
