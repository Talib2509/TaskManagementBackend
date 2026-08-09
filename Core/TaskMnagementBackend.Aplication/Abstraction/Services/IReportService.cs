using System;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.DTOs.Reporting;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    public interface IReportService
    {
        Task<PerformanceReportDataDto> GetTeamPerformanceDataAsync(int teamId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default);
        Task<PerformanceReportDataDto> GetUserPerformanceDataAsync(Guid userId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default);
        Task<PerformanceReportDataDto> GetCompanyPerformanceDataAsync(int companyId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default);

        Task<byte[]> ExportExcelAsync(PerformanceReportDataDto data);
        Task<byte[]> ExportPdfAsync(PerformanceReportDataDto data);
    }
}
