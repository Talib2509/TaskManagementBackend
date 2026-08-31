namespace TaskMnagementBackend.Aplication.DTOs.Role;

public sealed class RoleUserDto
{
    public Guid Id { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? FullName { get; init; }
}
