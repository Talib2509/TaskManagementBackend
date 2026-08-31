using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Domain.Entities.Identity;
using TaskMnagementBackend.Domain.Enums;
using UnitOfWorkContract = global::TaskMnagementBackend.Aplication.IUnitOfWork.IUnitOfWork;

namespace TaskMnagementBackend.Aplication.Features.Auth.Commands.Register
{
    public class RegisterHandler : IRequestHandler<RegisterRequest, RegisterResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IAuditLogService _auditLogService;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;
        private readonly UnitOfWorkContract _unitOfWork;

        public RegisterHandler(
            UserManager<AppUser> userManager,
            IEmailService emailService,
            IConfiguration configuration,
            IAuditLogService auditLogService,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor,
            UnitOfWorkContract unitOfWork)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegisterResponse> Handle(
            RegisterRequest request,
            CancellationToken cancellationToken)
        {
            var existUser = await _userManager.FindByEmailAsync(request.Email);

            if (existUser is not null)
            {
                return new RegisterResponse
                {
                    Succeeded = false,
                    Message = "Bu email artıq istifadə olunub."
                };
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var user = new AppUser
                {
                    Id = Guid.NewGuid(),
                    FullName = request.FullName,
                    Email = request.Email,
                    UserName = request.Email,
                    CompanyName = request.CompanyName,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                    EmailConfirmed = false
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    await _unitOfWork.RollbackAsync(CancellationToken.None);
                    return new RegisterResponse
                    {
                        Succeeded = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }

                var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.User);

                if (!roleResult.Succeeded)
                {
                    await _unitOfWork.RollbackAsync(CancellationToken.None);
                    return new RegisterResponse
                    {
                        Succeeded = false,
                        Message = string.Join(", ", roleResult.Errors.Select(e => e.Description))
                    };
                }

                var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                await _auditLogService.LogAsync(
                    action: "RoleAssigned",
                    entityType: "AppUser",
                    entityId: user.Id.ToString(),
                    details: $"Assigned role '{UserRoles.User}' to new user {user.Email} during registration.",
                    userId: user.Id,
                    userEmail: user.Email,
                    userName: user.UserName,
                    ipAddress: ip);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = Uri.EscapeDataString(token);

                var apiBaseUrl = _configuration["AppSettings:ApiBaseUrl"] ?? "https://localhost:7000";

                var confirmationLink =
                    $"{apiBaseUrl.TrimEnd('/')}/api/auth/confirm-email?userId={user.Id}&token={encodedToken}";

                var htmlBody = $@"
            <h2>CampusConnect Email Təsdiqi</h2>
            <p>Salam, {user.FullName}</p>
            <p>Hesabınızı aktivləşdirmək üçün aşağıdakı linkə klik edin:</p>
            <a href='{confirmationLink}'>Emaili təsdiqlə</a>
        ";

                await _emailService.SendEmailAsync(
                    user.Email!,
                    "CampusConnect - Email təsdiqi",
                    htmlBody);

                await _unitOfWork.CommitAsync(cancellationToken);

                return new RegisterResponse
                {
                    Succeeded = true,
                    Message = "Qeydiyyat uğurludur. Email təsdiq linki göndərildi."
                };
            }
            catch
            {
                await _unitOfWork.RollbackAsync(CancellationToken.None);
                throw;
            }
        }
    }
}
