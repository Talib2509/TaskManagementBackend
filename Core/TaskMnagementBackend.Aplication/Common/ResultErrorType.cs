using System;

namespace TaskMnagementBackend.Aplication.Common
{
    /// <summary>
    /// Bütün Command/Query Response-larında istifadə olunan standart xəta növləri.
    /// </summary>
    public enum ResultErrorType
    {
        None = 0,
        Validation,
        BadRequest,
        NotFound,
        Forbidden,
        Conflict,
        Error
    }
}
