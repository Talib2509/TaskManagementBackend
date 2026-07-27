using System;

namespace TaskMnagementBackend.Aplication.DTOs.Company
{
    public class CreateCompanyDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid OwnerId { get; set; }
    }
}
