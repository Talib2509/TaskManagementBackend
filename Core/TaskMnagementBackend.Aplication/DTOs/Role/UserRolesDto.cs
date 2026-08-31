namespace TaskMnagementBackend.Aplication.DTOs.Role;

public sealed class UserRolesDto
{
    public Guid UserId { get; init; }
    public IReadOnlyCollection<string> Roles { get; init; } = Array.Empty<string>();
}
