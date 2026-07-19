using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordRequest : IRequest<ForgotPasswordResponse>
    {
        public string Email { get; set; }
    }
}
