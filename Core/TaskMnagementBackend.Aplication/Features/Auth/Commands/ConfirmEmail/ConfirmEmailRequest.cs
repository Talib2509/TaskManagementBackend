using MediatR;

namespace TaskMnagementBackend.Aplication.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailRequest : IRequest<ConfirmEmailResponse>
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = default!;
    }
}
