using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.DTOs.TaskItem;
using TaskMnagementBackend.Aplication.IUnitOfWork;
using TaskMnagementBackend.Domain.Entities;
using TaskMnagementBackend.Domain.Enums;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TaskItemService(IUnitOfWork unitOfWork, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IQueryable<TaskItemDto> GetAll()
        {
            return _unitOfWork.TaskItemReadRepository
                .GetAll()
                .Select(x => new TaskItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    TeamId = x.TeamId,
                    AssignedUserId = x.AssignedUserId,
                    IsPrivate = x.IsPrivate,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    CompletedAt = x.CompletedAt
                });
        }

        public async Task<TaskItemDto?> GetByIdAsync(int id)
        {
            var task = await _unitOfWork.TaskItemReadRepository.GetByIdAsync(id);

            if (task == null)
                return null;

            return new TaskItemDto
            {
                Id = task.Id,
                Title = task.Title,
                TeamId = task.TeamId,
                AssignedUserId = task.AssignedUserId,
                IsPrivate = task.IsPrivate,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                CompletedAt = task.CompletedAt
            };
        }

        public async Task<IEnumerable<TaskItemDto>> GetByTeamAsync(int teamId)
        {
            return await _unitOfWork.TaskItemReadRepository
                .GetWhere(x => x.TeamId == teamId)
                .Select(x => new TaskItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    TeamId = x.TeamId,
                    AssignedUserId = x.AssignedUserId,
                    IsPrivate = x.IsPrivate,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    CompletedAt = x.CompletedAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItemDto>> GetMyTasksAsync(Guid userId)
        {
            return await _unitOfWork.TaskItemReadRepository
                .GetWhere(x => x.AssignedUserId == userId)
                .Select(x => new TaskItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    TeamId = x.TeamId,
                    AssignedUserId = x.AssignedUserId,
                    IsPrivate = x.IsPrivate,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    CompletedAt = x.CompletedAt
                })
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(CreateTaskItemDto dto)
        {
            var entity = new TaskItem
            {
                Title = dto.Title,
                TeamId = dto.TeamId,
                AssignedUserId = dto.AssignedUserId,
                IsPrivate = dto.IsPrivate,
                Status = TaskItemStatus.Todo,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.TaskItemWriteRepository.AddAsync(entity);

            var res = await _unitOfWork.SaveChangesAsync() > 0;

            if (res)
            {
                try
                {
                    var user = _httpContextAccessor.HttpContext?.User;
                    var userIdStr = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user?.FindFirst("UserId")?.Value;
                    Guid.TryParse(userIdStr, out var uid);

                    await _auditLogService.LogAsync(
                        action: "TaskCreated",
                        entityType: "TaskItem",
                        entityId: entity.Id.ToString(),
                        details: $"Task '{entity.Title}' ({entity.Id}) created in team {entity.TeamId}.",
                        userId: uid,
                        userEmail: user?.FindFirst(ClaimTypes.Email)?.Value,
                        userName: user?.Identity?.Name,
                        ipAddress: _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
                }
                catch { }
            }

            return res;
        }

        public async Task<bool> UpdateAsync(UpdateTaskItemDto dto)
        {
            var entity = await _unitOfWork.TaskItemReadRepository.GetByIdAsync(dto.Id);

            if (entity == null)
                return false;

            entity.Title = dto.Title;
            entity.TeamId = dto.TeamId;
            entity.AssignedUserId = dto.AssignedUserId;
            entity.IsPrivate = dto.IsPrivate;
            entity.Status = dto.Status;

            if (dto.Status == TaskItemStatus.Done)
                entity.CompletedAt = DateTime.UtcNow;
            else
                entity.CompletedAt = null;

            _unitOfWork.TaskItemWriteRepository.Update(entity);

            var res = await _unitOfWork.SaveChangesAsync() > 0;

            if (res)
            {
                try
                {
                    var user = _httpContextAccessor.HttpContext?.User;
                    var userIdStr = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user?.FindFirst("UserId")?.Value;
                    Guid.TryParse(userIdStr, out var uid);

                    await _auditLogService.LogAsync(
                        action: "TaskUpdated",
                        entityType: "TaskItem",
                        entityId: entity.Id.ToString(),
                        details: $"Task '{entity.Title}' ({entity.Id}) updated.",
                        userId: uid,
                        userEmail: user?.FindFirst(ClaimTypes.Email)?.Value,
                        userName: user?.Identity?.Name,
                        ipAddress: _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
                }
                catch { }
            }

            return res;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _unitOfWork.TaskItemWriteRepository.DeleteAsync(id);

            if (!result)
                return false;

            var res = await _unitOfWork.SaveChangesAsync() > 0;

            if (res)
            {
                try
                {
                    var user = _httpContextAccessor.HttpContext?.User;
                    var userIdStr = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user?.FindFirst("UserId")?.Value;
                    Guid.TryParse(userIdStr, out var uid);

                    await _auditLogService.LogAsync(
                        action: "TaskDeleted",
                        entityType: "TaskItem",
                        entityId: id.ToString(),
                        details: $"Task {id} deleted.",
                        userId: uid,
                        userEmail: user?.FindFirst(ClaimTypes.Email)?.Value,
                        userName: user?.Identity?.Name,
                        ipAddress: _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
                }
                catch { }
            }

            return res;
        }

        public async Task<bool> ChangeStatusAsync(int id, TaskItemStatus status)
        {
            var entity = await _unitOfWork.TaskItemReadRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            entity.Status = status;

            if (status == TaskItemStatus.Done)
                entity.CompletedAt = DateTime.UtcNow;
            else
                entity.CompletedAt = null;

            _unitOfWork.TaskItemWriteRepository.Update(entity);

            var res = await _unitOfWork.SaveChangesAsync() > 0;

            if (res)
            {
                try
                {
                    var user = _httpContextAccessor.HttpContext?.User;
                    var userIdStr = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user?.FindFirst("UserId")?.Value;
                    Guid.TryParse(userIdStr, out var uid);

                    await _auditLogService.LogAsync(
                        action: "TaskStatusChanged",
                        entityType: "TaskItem",
                        entityId: entity.Id.ToString(),
                        details: $"Task {entity.Id} status changed to {status}.",
                        userId: uid,
                        userEmail: user?.FindFirst(ClaimTypes.Email)?.Value,
                        userName: user?.Identity?.Name,
                        ipAddress: _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
                }
                catch { }
            }

            return res;
        }

        public async Task<bool> AssignMemberAsync(int taskId, Guid userId)
        {
            var entity = await _unitOfWork.TaskItemReadRepository.GetByIdAsync(taskId);

            if (entity == null)
                return false;

            var member = await _unitOfWork.TeamMemberReadRepository
                .GetSingleAsync(x => x.TeamId == entity.TeamId &&
                                     x.UserId == userId);

            if (member == null)
                return false;

            entity.AssignedUserId = userId;

            _unitOfWork.TaskItemWriteRepository.Update(entity);

            var res = await _unitOfWork.SaveChangesAsync() > 0;

            if (res)
            {
                try
                {
                    var user = _httpContextAccessor.HttpContext?.User;
                    var userIdStr = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user?.FindFirst("UserId")?.Value;
                    Guid.TryParse(userIdStr, out var uid);

                    await _auditLogService.LogAsync(
                        action: "TaskAssigned",
                        entityType: "TaskItem",
                        entityId: entity.Id.ToString(),
                        details: $"Task {entity.Id} assigned to user {userId}.",
                        userId: uid,
                        userEmail: user?.FindFirst(ClaimTypes.Email)?.Value,
                        userName: user?.Identity?.Name,
                        ipAddress: _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
                }
                catch { }
            }

            return res;
        }
    }
}