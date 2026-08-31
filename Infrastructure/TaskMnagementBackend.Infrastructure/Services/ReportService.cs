using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.DTOs.Reporting;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Domain.Entities.Identity;
using TaskMnagementBackend.Domain.Enums;
using TaskMnagementBackend.Persistence.Context;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public ReportService(AppDbContext dbContext, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;

            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<PerformanceReportDataDto> GetTeamPerformanceDataAsync(
            int teamId,
            Guid requestingUserId,
            DateTime? fromDate,
            DateTime? toDate,
            CancellationToken cancellationToken = default)
        {
            var team = await _dbContext.Teams
                .Include(t => t.Company)
                .Include(t => t.TeamMembers.Where(m => m.IsActive))
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(t => t.Id == teamId && !t.IsDeleted, cancellationToken);

            if (team == null)
                throw new Exception("Komanda tapılmadı.");

            // Authorization: only admins, superadmins, company owner or team members can access
            var requestingUser = await _userManager.FindByIdAsync(requestingUserId.ToString());
            var isAdmin = requestingUser != null && (await _userManager.IsInRoleAsync(requestingUser, UserRoles.Admin) || await _userManager.IsInRoleAsync(requestingUser, UserRoles.SuperAdmin));
            var isCompanyOwner = team.Company != null && team.Company.OwnerId == requestingUserId;
            var isTeamMember = team.TeamMembers.Any(m => m.UserId == requestingUserId && m.IsActive);

            if (!isAdmin && !isCompanyOwner && !isTeamMember)
                throw new UnauthorizedAccessException("Bu komandanın hesabatına giriş icazəniz yoxdur.");

            var query = _dbContext.TaskItems
                .Include(t => t.AssignedUser)
                .Where(t => t.TeamId == teamId);

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CreatedAt <= toDate.Value);

            var tasks = await query.ToListAsync(cancellationToken);
            var now = DateTime.UtcNow;

            var reportTasks = tasks.Select(t => new ReportTaskItemDto
            {
                Id = t.Id,
                Title = t.Title,
                Status = t.Status.ToString(),
                Priority = t.Priority.ToString(),
                AssignedUserName = t.AssignedUser?.FullName ?? t.AssignedUser?.UserName,
                DueDate = t.DueDate,
                CompletedAt = t.CompletedAt,
                CreatedAt = t.CreatedAt,
                IsOverdue = t.Status != TaskItemStatus.Done && t.DueDate.HasValue && t.DueDate.Value < now
            }).ToList();

            var members = new List<ReportMemberPerformanceDto>();
            foreach (var member in team.TeamMembers)
            {
                if (member.User == null) continue;

                var memberTasks = tasks.Where(t => t.AssignedUserId == member.UserId).ToList();
                var mAssigned = memberTasks.Count;
                var mCompleted = memberTasks.Count(t => t.Status == TaskItemStatus.Done);
                var mOverdue = memberTasks.Count(t => t.Status != TaskItemStatus.Done && t.DueDate.HasValue && t.DueDate.Value < now);

                members.Add(new ReportMemberPerformanceDto
                {
                    UserId = member.UserId,
                    FullName = member.User.FullName ?? member.User.UserName ?? "Üzv",
                    Email = member.User.Email ?? string.Empty,
                    Role = member.Role.ToString(),
                    AssignedTasks = mAssigned,
                    CompletedTasks = mCompleted,
                    OverdueTasks = mOverdue,
                    CompletionRate = mAssigned > 0 ? Math.Round((double)mCompleted / mAssigned * 100, 2) : 0.0
                });
            }

            var totalTasks = reportTasks.Count;
            var completedCount = reportTasks.Count(t => t.Status == TaskItemStatus.Done.ToString());
            var inProgressCount = reportTasks.Count(t => t.Status == TaskItemStatus.InProgress.ToString());
            var pendingCount = reportTasks.Count(t => t.Status == TaskItemStatus.Todo.ToString());
            var overdueCount = reportTasks.Count(t => t.IsOverdue);

            return new PerformanceReportDataDto
            {
                Title = $"{team.Name} - Komanda Performans Hesabatı",
                ScopeName = team.Name,
                ScopeType = "Team",
                GeneratedAt = DateTime.UtcNow,
                FromDate = fromDate,
                ToDate = toDate,
                TotalTasks = totalTasks,
                CompletedTasks = completedCount,
                InProgressTasks = inProgressCount,
                PendingTasks = pendingCount,
                OverdueTasks = overdueCount,
                CompletionPercentage = totalTasks > 0 ? Math.Round((double)completedCount / totalTasks * 100, 2) : 0.0,
                OverduePercentage = totalTasks > 0 ? Math.Round((double)overdueCount / totalTasks * 100, 2) : 0.0,
                Tasks = reportTasks,
                Members = members
            };
        }

        public async Task<PerformanceReportDataDto> GetUserPerformanceDataAsync(
            Guid userId,
            Guid requestingUserId,
            DateTime? fromDate,
            DateTime? toDate,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                throw new Exception("İstifadəçi tapılmadı.");

            // Authorization: allow if the requester is the same user or admin/superadmin.
            // Additionally, allow a company owner to view reports for users who belong to their company.
            if (requestingUserId != userId)
            {
                var requestingUser = await _userManager.FindByIdAsync(requestingUserId.ToString());
                var isAdmin = requestingUser != null && (await _userManager.IsInRoleAsync(requestingUser, UserRoles.Admin) || await _userManager.IsInRoleAsync(requestingUser, UserRoles.SuperAdmin));

                if (!isAdmin)
                {
                    // Check if requester is owner of a company and the target user belongs to that company
                    var ownerCompany = await _dbContext.Companies.FirstOrDefaultAsync(c => c.OwnerId == requestingUserId && !c.IsDeleted, cancellationToken);
                    if (ownerCompany == null)
                        throw new UnauthorizedAccessException("Bu istifadəçinin hesabatını görmək üçün icazəniz yoxdur.");

                    var isUserInCompany = await _dbContext.TeamMembers
                        .Include(m => m.Team)
                        .AnyAsync(m => m.UserId == userId && m.IsActive && m.Team != null && !m.Team.IsDeleted && m.Team.CompanyId == ownerCompany.Id, cancellationToken);

                    if (!isUserInCompany)
                        throw new UnauthorizedAccessException("Bu istifadəçinin hesabatını görmək üçün icazəniz yoxdur.");
                }
            }

            var query = _dbContext.TaskItems
                .Include(t => t.Team)
                .Where(t => t.AssignedUserId == userId);

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CreatedAt <= toDate.Value);

            var tasks = await query.ToListAsync(cancellationToken);
            var now = DateTime.UtcNow;

            var reportTasks = tasks.Select(t => new ReportTaskItemDto
            {
                Id = t.Id,
                Title = t.Title,
                Status = t.Status.ToString(),
                Priority = t.Priority.ToString(),
                AssignedUserName = user.FullName ?? user.UserName,
                DueDate = t.DueDate,
                CompletedAt = t.CompletedAt,
                CreatedAt = t.CreatedAt,
                IsOverdue = t.Status != TaskItemStatus.Done && t.DueDate.HasValue && t.DueDate.Value < now
            }).ToList();

            var totalTasks = reportTasks.Count;
            var completedCount = reportTasks.Count(t => t.Status == TaskItemStatus.Done.ToString());
            var inProgressCount = reportTasks.Count(t => t.Status == TaskItemStatus.InProgress.ToString());
            var pendingCount = reportTasks.Count(t => t.Status == TaskItemStatus.Todo.ToString());
            var overdueCount = reportTasks.Count(t => t.IsOverdue);

            return new PerformanceReportDataDto
            {
                Title = $"{user.FullName ?? user.UserName} - İstifadəçi Performans Hesabatı",
                ScopeName = user.FullName ?? user.UserName ?? "İstifadəçi",
                ScopeType = "User",
                GeneratedAt = DateTime.UtcNow,
                FromDate = fromDate,
                ToDate = toDate,
                TotalTasks = totalTasks,
                CompletedTasks = completedCount,
                InProgressTasks = inProgressCount,
                PendingTasks = pendingCount,
                OverdueTasks = overdueCount,
                CompletionPercentage = totalTasks > 0 ? Math.Round((double)completedCount / totalTasks * 100, 2) : 0.0,
                OverduePercentage = totalTasks > 0 ? Math.Round((double)overdueCount / totalTasks * 100, 2) : 0.0,
                Tasks = reportTasks,
                Members = new List<ReportMemberPerformanceDto>()
            };
        }

        public async Task<PerformanceReportDataDto> GetCompanyPerformanceDataAsync(
            int companyId,
            Guid requestingUserId,
            DateTime? fromDate,
            DateTime? toDate,
            CancellationToken cancellationToken = default)
        {
            var company = await _dbContext.Companies
                .Include(c => c.Teams.Where(t => !t.IsDeleted))
                    .ThenInclude(t => t.TeamMembers.Where(m => m.IsActive))
                        .ThenInclude(m => m.User)
                .Include(c => c.Teams.Where(t => !t.IsDeleted))
                    .ThenInclude(t => t.TaskItems)
                        .ThenInclude(ti => ti.AssignedUser)
                .FirstOrDefaultAsync(c => c.Id == companyId && !c.IsDeleted, cancellationToken);

            if (company == null)
                throw new Exception("Şirkət tapılmadı.");

            var requestingUser = await _userManager.FindByIdAsync(requestingUserId.ToString());
            var isAdmin = requestingUser != null &&
                (await _userManager.IsInRoleAsync(requestingUser, UserRoles.Admin) ||
                 await _userManager.IsInRoleAsync(requestingUser, UserRoles.SuperAdmin));

            if (!isAdmin && company.OwnerId != requestingUserId)
                throw new UnauthorizedAccessException("Bu şirkətin hesabatına giriş icazəniz yoxdur.");

            var allTasks = company.Teams.SelectMany(t => t.TaskItems).AsQueryable();

            if (fromDate.HasValue)
                allTasks = allTasks.Where(t => t.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                allTasks = allTasks.Where(t => t.CreatedAt <= toDate.Value);

            var taskList = allTasks.ToList();
            var now = DateTime.UtcNow;

            var reportTasks = taskList.Select(t => new ReportTaskItemDto
            {
                Id = t.Id,
                Title = t.Title,
                Status = t.Status.ToString(),
                Priority = t.Priority.ToString(),
                AssignedUserName = t.AssignedUser?.FullName ?? t.AssignedUser?.UserName,
                DueDate = t.DueDate,
                CompletedAt = t.CompletedAt,
                CreatedAt = t.CreatedAt,
                IsOverdue = t.Status != TaskItemStatus.Done && t.DueDate.HasValue && t.DueDate.Value < now
            }).ToList();

            var allMembers = company.Teams.SelectMany(t => t.TeamMembers).Where(m => m.IsActive && m.User != null).ToList();
            var distinctMembers = allMembers.GroupBy(m => m.UserId).Select(g => g.First()).ToList();

            var members = new List<ReportMemberPerformanceDto>();
            foreach (var member in distinctMembers)
            {
                var memberTasks = taskList.Where(t => t.AssignedUserId == member.UserId).ToList();
                var mAssigned = memberTasks.Count;
                var mCompleted = memberTasks.Count(t => t.Status == TaskItemStatus.Done);
                var mOverdue = memberTasks.Count(t => t.Status != TaskItemStatus.Done && t.DueDate.HasValue && t.DueDate.Value < now);

                members.Add(new ReportMemberPerformanceDto
                {
                    UserId = member.UserId,
                    FullName = member.User!.FullName ?? member.User.UserName ?? "Üzv",
                    Email = member.User.Email ?? string.Empty,
                    Role = member.Role.ToString(),
                    AssignedTasks = mAssigned,
                    CompletedTasks = mCompleted,
                    OverdueTasks = mOverdue,
                    CompletionRate = mAssigned > 0 ? Math.Round((double)mCompleted / mAssigned * 100, 2) : 0.0
                });
            }

            var totalTasks = reportTasks.Count;
            var completedCount = reportTasks.Count(t => t.Status == TaskItemStatus.Done.ToString());
            var inProgressCount = reportTasks.Count(t => t.Status == TaskItemStatus.InProgress.ToString());
            var pendingCount = reportTasks.Count(t => t.Status == TaskItemStatus.Todo.ToString());
            var overdueCount = reportTasks.Count(t => t.IsOverdue);

            return new PerformanceReportDataDto
            {
                Title = $"{company.Name} - Şirkət Performans Hesabatı",
                ScopeName = company.Name,
                ScopeType = "Company",
                GeneratedAt = DateTime.UtcNow,
                FromDate = fromDate,
                ToDate = toDate,
                TotalTasks = totalTasks,
                CompletedTasks = completedCount,
                InProgressTasks = inProgressCount,
                PendingTasks = pendingCount,
                OverdueTasks = overdueCount,
                CompletionPercentage = totalTasks > 0 ? Math.Round((double)completedCount / totalTasks * 100, 2) : 0.0,
                OverduePercentage = totalTasks > 0 ? Math.Round((double)overdueCount / totalTasks * 100, 2) : 0.0,
                Tasks = reportTasks,
                Members = members
            };
        }

        public Task<byte[]> ExportExcelAsync(PerformanceReportDataDto data)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Performans Hesabatı");

            // Header Style
            ws.Cell(1, 1).Value = data.Title;
            ws.Cell(1, 1).Style.Font.Bold = true;
            ws.Cell(1, 1).Style.Font.FontSize = 16;
            ws.Cell(1, 1).Style.Font.FontColor = XLColor.FromHtml("#1E293B");

            var dateRangeStr = $"Tarix Aralığı: {(data.FromDate.HasValue ? data.FromDate.Value.ToString("dd.MM.yyyy") : "Əvvəldən")} - {(data.ToDate.HasValue ? data.ToDate.Value.ToString("dd.MM.yyyy") : "İndiyə qədər")}";
            ws.Cell(2, 1).Value = dateRangeStr;
            ws.Cell(2, 1).Style.Font.Italic = true;
            ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;

            ws.Cell(3, 1).Value = $"Hazırlanma Tarixi: {data.GeneratedAt:dd.MM.yyyy HH:mm:ss} (UTC)";
            ws.Cell(3, 1).Style.Font.FontColor = XLColor.Gray;

            // KPI Summary Table
            int row = 5;
            ws.Cell(row, 1).Value = "Göstərici";
            ws.Cell(row, 2).Value = "Dəyər";
            var kpiHeader = ws.Range(row, 1, row, 2);
            kpiHeader.Style.Font.Bold = true;
            kpiHeader.Style.Fill.BackgroundColor = XLColor.FromHtml("#3B82F6");
            kpiHeader.Style.Font.FontColor = XLColor.White;

            row++;
            ws.Cell(row, 1).Value = "Cəmi Tapşırıqlar";
            ws.Cell(row, 2).Value = data.TotalTasks;
            row++;
            ws.Cell(row, 1).Value = "Tamamlanan Tapşırıqlar";
            ws.Cell(row, 2).Value = data.CompletedTasks;
            row++;
            ws.Cell(row, 1).Value = "İcrada Olan Tapşırıqlar";
            ws.Cell(row, 2).Value = data.InProgressTasks;
            row++;
            ws.Cell(row, 1).Value = "Gözləmədə Olan Tapşırıqlar";
            ws.Cell(row, 2).Value = data.PendingTasks;
            row++;
            ws.Cell(row, 1).Value = "Gecikmiş Tapşırıqlar";
            ws.Cell(row, 2).Value = data.OverdueTasks;
            row++;
            ws.Cell(row, 1).Value = "Tamamlanma Faizi";
            ws.Cell(row, 2).Value = $"{data.CompletionPercentage}%";
            row++;
            ws.Cell(row, 1).Value = "Gecikmə Faizi";
            ws.Cell(row, 2).Value = $"{data.OverduePercentage}%";

            ws.Range(5, 1, row, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range(5, 1, row, 2).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            // Members Table (if any)
            if (data.Members != null && data.Members.Any())
            {
                row += 2;
                ws.Cell(row, 1).Value = "Komanda Üzvlərinin Performansı";
                ws.Cell(row, 1).Style.Font.Bold = true;
                ws.Cell(row, 1).Style.Font.FontSize = 13;

                row++;
                string[] memberHeaders = { "Ad Soyad", "Email", "Rol", "Təyin Edilmiş", "Tamamlanan", "Gecikmiş", "Uğur Faizi" };
                for (int c = 0; c < memberHeaders.Length; c++)
                {
                    ws.Cell(row, c + 1).Value = memberHeaders[c];
                }
                var mHeaderRange = ws.Range(row, 1, row, memberHeaders.Length);
                mHeaderRange.Style.Font.Bold = true;
                mHeaderRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#10B981");
                mHeaderRange.Style.Font.FontColor = XLColor.White;

                int memberStartRow = row;
                foreach (var m in data.Members)
                {
                    row++;
                    ws.Cell(row, 1).Value = m.FullName;
                    ws.Cell(row, 2).Value = m.Email;
                    ws.Cell(row, 3).Value = m.Role ?? "-";
                    ws.Cell(row, 4).Value = m.AssignedTasks;
                    ws.Cell(row, 5).Value = m.CompletedTasks;
                    ws.Cell(row, 6).Value = m.OverdueTasks;
                    ws.Cell(row, 7).Value = $"{m.CompletionRate}%";
                }
                ws.Range(memberStartRow, 1, row, memberHeaders.Length).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(memberStartRow, 1, row, memberHeaders.Length).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            }

            // Tasks Table
            if (data.Tasks != null && data.Tasks.Any())
            {
                row += 2;
                ws.Cell(row, 1).Value = "Tapşırıqların Siyahısı";
                ws.Cell(row, 1).Style.Font.Bold = true;
                ws.Cell(row, 1).Style.Font.FontSize = 13;

                row++;
                string[] taskHeaders = { "ID", "Başlıq", "Status", "Prioritet", "İcraçı", "Bitmə Tarixi", "Tamamlanma Tarixi", "Gecikib?" };
                for (int c = 0; c < taskHeaders.Length; c++)
                {
                    ws.Cell(row, c + 1).Value = taskHeaders[c];
                }
                var tHeaderRange = ws.Range(row, 1, row, taskHeaders.Length);
                tHeaderRange.Style.Font.Bold = true;
                tHeaderRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#6366F1");
                tHeaderRange.Style.Font.FontColor = XLColor.White;

                int taskStartRow = row;
                foreach (var t in data.Tasks)
                {
                    row++;
                    ws.Cell(row, 1).Value = t.Id;
                    ws.Cell(row, 2).Value = t.Title;
                    ws.Cell(row, 3).Value = t.Status;
                    ws.Cell(row, 4).Value = t.Priority;
                    ws.Cell(row, 5).Value = t.AssignedUserName ?? "Təyin olunmayıb";
                    ws.Cell(row, 6).Value = t.DueDate.HasValue ? t.DueDate.Value.ToString("dd.MM.yyyy HH:mm") : "-";
                    ws.Cell(row, 7).Value = t.CompletedAt.HasValue ? t.CompletedAt.Value.ToString("dd.MM.yyyy HH:mm") : "-";
                    ws.Cell(row, 8).Value = t.IsOverdue ? "Bəli" : "Xeyr";

                    if (t.IsOverdue)
                    {
                        ws.Cell(row, 8).Style.Font.FontColor = XLColor.Red;
                        ws.Cell(row, 8).Style.Font.Bold = true;
                    }
                }
                ws.Range(taskStartRow, 1, row, taskHeaders.Length).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(taskStartRow, 1, row, taskHeaders.Length).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return Task.FromResult(stream.ToArray());
        }

        public Task<byte[]> ExportPdfAsync(PerformanceReportDataDto data)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    // Header
                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(data.Title).FontSize(16).Bold().FontColor(Colors.Blue.Darken2);
                                c.Item().Text($"Əhatə: {data.ScopeName} ({data.ScopeType})").FontSize(11).SemiBold().FontColor(Colors.Grey.Darken1);
                                var dateRangeStr = $"Tarix Aralığı: {(data.FromDate.HasValue ? data.FromDate.Value.ToString("dd.MM.yyyy") : "Əvvəldən")} - {(data.ToDate.HasValue ? data.ToDate.Value.ToString("dd.MM.yyyy") : "İndiyə qədər")}";
                                c.Item().Text(dateRangeStr).FontSize(9).Italic().FontColor(Colors.Grey.Medium);
                            });

                            row.ConstantItem(120).AlignRight().Column(c =>
                            {
                                c.Item().Text("Task Management").Bold().FontSize(12).FontColor(Colors.Blue.Medium);
                                c.Item().Text($"Tarix: {data.GeneratedAt:dd.MM.yyyy}").FontSize(8).FontColor(Colors.Grey.Darken1);
                            });
                        });

                        col.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                    // Content
                    page.Content().PaddingVertical(15).Column(col =>
                    {
                        // KPI Cards
                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Border(1).BorderColor(Colors.Blue.Lighten3).Background(Colors.Blue.Lighten5).Padding(8).Column(c =>
                            {
                                c.Item().Text("Cəmi").FontSize(9).FontColor(Colors.Grey.Darken2);
                                c.Item().Text(data.TotalTasks.ToString()).FontSize(14).Bold().FontColor(Colors.Blue.Darken2);
                            });

                            r.ConstantItem(10);

                            r.RelativeItem().Border(1).BorderColor(Colors.Green.Lighten3).Background(Colors.Green.Lighten5).Padding(8).Column(c =>
                            {
                                c.Item().Text("Tamamlanan").FontSize(9).FontColor(Colors.Grey.Darken2);
                                c.Item().Text(data.CompletedTasks.ToString()).FontSize(14).Bold().FontColor(Colors.Green.Darken2);
                            });

                            r.ConstantItem(10);

                            r.RelativeItem().Border(1).BorderColor(Colors.Orange.Lighten3).Background(Colors.Orange.Lighten5).Padding(8).Column(c =>
                            {
                                c.Item().Text("İcrada").FontSize(9).FontColor(Colors.Grey.Darken2);
                                c.Item().Text(data.InProgressTasks.ToString()).FontSize(14).Bold().FontColor(Colors.Orange.Darken2);
                            });

                            r.ConstantItem(10);

                            r.RelativeItem().Border(1).BorderColor(Colors.Red.Lighten3).Background(Colors.Red.Lighten5).Padding(8).Column(c =>
                            {
                                c.Item().Text("Gecikmiş").FontSize(9).FontColor(Colors.Grey.Darken2);
                                c.Item().Text(data.OverdueTasks.ToString()).FontSize(14).Bold().FontColor(Colors.Red.Darken2);
                            });

                            r.ConstantItem(10);

                            r.RelativeItem().Border(1).BorderColor(Colors.Purple.Lighten3).Background(Colors.Purple.Lighten5).Padding(8).Column(c =>
                            {
                                c.Item().Text("Uğur %").FontSize(9).FontColor(Colors.Grey.Darken2);
                                c.Item().Text($"{data.CompletionPercentage}%").FontSize(14).Bold().FontColor(Colors.Purple.Darken2);
                            });
                        });

                        // Members Table
                        if (data.Members != null && data.Members.Any())
                        {
                            col.Item().PaddingTop(20).Text("Komanda Üzvlərinin Performansı").FontSize(12).Bold().FontColor(Colors.Grey.Darken3);
                            col.Item().PaddingTop(5).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Green.Darken1).Padding(5).Text("Ad Soyad").Bold().FontColor(Colors.White);
                                    header.Cell().Background(Colors.Green.Darken1).Padding(5).Text("Email").Bold().FontColor(Colors.White);
                                    header.Cell().Background(Colors.Green.Darken1).Padding(5).Text("Rol").Bold().FontColor(Colors.White);
                                    header.Cell().Background(Colors.Green.Darken1).Padding(5).Text("Təyin").Bold().FontColor(Colors.White);
                                    header.Cell().Background(Colors.Green.Darken1).Padding(5).Text("Tamamlanan").Bold().FontColor(Colors.White);
                                    header.Cell().Background(Colors.Green.Darken1).Padding(5).Text("Uğur %").Bold().FontColor(Colors.White);
                                });

                                foreach (var m in data.Members)
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4).Text(m.FullName);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4).Text(m.Email).FontSize(8);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4).Text(m.Role ?? "-");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4).Text(m.AssignedTasks.ToString());
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4).Text(m.CompletedTasks.ToString());
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4).Text($"{m.CompletionRate}%").Bold();
                                }
                            });
                        }

                        // Tasks Table
                        if (data.Tasks != null && data.Tasks.Any())
                        {
                            col.Item().PaddingTop(20).Text("Tapşırıqların Detallı Siyahısı").FontSize(12).Bold().FontColor(Colors.Grey.Darken3);
                            col.Item().PaddingTop(5).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(30);
                                    columns.RelativeColumn(4);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(3);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Indigo.Darken1).Padding(5).Text("#").Bold().FontColor(Colors.White);
                                    header.Cell().Background(Colors.Indigo.Darken1).Padding(5).Text("Başlıq").Bold().FontColor(Colors.White);
                                    header.Cell().Background(Colors.Indigo.Darken1).Padding(5).Text("Status").Bold().FontColor(Colors.White);
                                    header.Cell().Background(Colors.Indigo.Darken1).Padding(5).Text("Prioritet").Bold().FontColor(Colors.White);
                                    header.Cell().Background(Colors.Indigo.Darken1).Padding(5).Text("İcraçı").Bold().FontColor(Colors.White);
                                    header.Cell().Background(Colors.Indigo.Darken1).Padding(5).Text("Bitmə Tarixi").Bold().FontColor(Colors.White);
                                });

                                foreach (var t in data.Tasks.Take(50))
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4).Text(t.Id.ToString());
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4).Text(t.Title).SemiBold();
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4).Text(t.Status);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4).Text(t.Priority);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4).Text(t.AssignedUserName ?? "-");
                                    
                                    var dueDateCell = table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(4);
                                    if (t.IsOverdue)
                                    {
                                        dueDateCell.Text(t.DueDate?.ToString("dd.MM.yyyy") ?? "-").Bold().FontColor(Colors.Red.Medium);
                                    }
                                    else
                                    {
                                        dueDateCell.Text(t.DueDate?.ToString("dd.MM.yyyy") ?? "-");
                                    }
                                }
                            });
                        }
                    });

                    // Footer
                    page.Footer().Row(r =>
                    {
                        r.RelativeItem().Text($"Hesabat avtomatik yaradılmışdır.").FontSize(8).FontColor(Colors.Grey.Medium);
                        r.RelativeItem().AlignRight().Text(x =>
                        {
                            x.Span("Səhifə ");
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        });
                    });
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return Task.FromResult(stream.ToArray());
        }

    }
}
