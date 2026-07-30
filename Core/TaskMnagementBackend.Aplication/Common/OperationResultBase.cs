using System;

namespace TaskMnagementBackend.Aplication.Common
{
    /// <summary>
    /// Bütün Command Response-ları üçün minimum standart baza sinif.
    /// Succeeded / Message / ErrorType layihə boyu bütün nəticələrdə eyni şəkildə istifadə olunur.
    /// </summary>
    public abstract class OperationResultBase
    {
        public bool Succeeded { get; set; }

        public string Message { get; set; } = string.Empty;

        public ResultErrorType? ErrorType { get; set; }
    }
}
