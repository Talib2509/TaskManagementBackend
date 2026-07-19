using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordRequest : IRequest<ResetPasswordResponse>
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
