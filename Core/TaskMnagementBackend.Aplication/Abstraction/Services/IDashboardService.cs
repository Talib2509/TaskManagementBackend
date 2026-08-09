using System;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.DTOs.Dashboard;

namespace TaskMnagementBackend.Aplication.Abstraction.Services
{
    public interface IDashboardService
    {
        Task<AdminDashboardStatsDto> GetAdminDashboardStatsAsync(int days = 30, CancellationToken cancellationToken = default);
        Task<CompanyDashboardStatsDto> GetCompanyDashboardStatsAsync(Guid userId, int? companyId = null, CancellationToken cancellationToken = default);
    }
}
