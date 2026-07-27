using System;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    /// <summary>
    /// HTTP kontekstindəki cari (autentifikasiya olunmuş) istifadəçi məlumatına
    /// Application Layer-dən çıxış üçün abstraksiya. Implementasiyası (HttpContextAccessor
    /// vasitəsilə Claim-lərin oxunması) Infrastructure/API layer-də olmalıdır.
    /// </summary>
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
    }
}
