using System.Linq;
using TaskMnagementBackend.Aplication.Abstraction.Services;
using TaskMnagementBackend.Aplication.DTOs.Company;
using TaskMnagementBackend.Aplication.IUnitOfWork;
using TaskMnagementBackend.Domain.Entities;

namespace TaskMnagementBackend.Infrastructure.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<CompanyDto> GetAll()
        {
            return _unitOfWork.CompanyReadRepository
                .GetAll()
                .Where(x => !x.IsDeleted)
                .Select(x => new CompanyDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    OwnerId = x.OwnerId,
                    CreatedAt = x.CreatedAt,
                    IsDeleted = x.IsDeleted,
                    DeletedAt = x.DeletedAt
                });
        }

        public async Task<CompanyDto?> GetByIdAsync(int id)
        {
            var company = await _unitOfWork.CompanyReadRepository.GetByIdAsync(id);

            if (company == null || company.IsDeleted)
                return null;

            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
                OwnerId = company.OwnerId,
                CreatedAt = company.CreatedAt,
                IsDeleted = company.IsDeleted,
                DeletedAt = company.DeletedAt
            };
        }

        public async Task<bool> CreateAsync(CreateCompanyDto dto)
        {
            var entity = new Company
            {
                Name = dto.Name,
                Description = dto.Description,
                OwnerId = dto.OwnerId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _unitOfWork.CompanyWriteRepository.AddAsync(entity);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> UpdateAsync(UpdateCompanyDto dto)
        {
            var entity = await _unitOfWork.CompanyReadRepository.GetByIdAsync(dto.Id);

            if (entity == null || entity.IsDeleted)
                return false;

            entity.Name = dto.Name;
            entity.Description = dto.Description;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                _unitOfWork.CompanyWriteRepository.Update(entity);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.CompanyReadRepository.GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return false;

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                _unitOfWork.CompanyWriteRepository.Update(entity);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<CompanyDto?> GetByOwnerIdAsync(Guid ownerId)
        {
            if (!Guid.TryParse(ownerId.ToString(), out Guid ownerGuid))
                return null;

            var company = await _unitOfWork.CompanyReadRepository
                .GetSingleAsync(x => x.OwnerId == ownerGuid && !x.IsDeleted);

            if (company == null)
                return null;

            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
                OwnerId = company.OwnerId,
                CreatedAt = company.CreatedAt,
                IsDeleted = company.IsDeleted,
                DeletedAt = company.DeletedAt
            };
        }

        public async Task<CompanyDto?> GetMyCompanyAsync(Guid ownerId)
        {
            return await GetByOwnerIdAsync(ownerId);
        }

        public async Task<CompanyStatisticsDto> GetStatisticsAsync(int companyId)
        {
            var company = await _unitOfWork.CompanyReadRepository.GetByIdAsync(companyId);

            if (company == null || company.IsDeleted)
                return new CompanyStatisticsDto();

            var teamCount = company.Teams?.Count ?? 0;

            var memberCount = company.Teams?
                .Sum(x => x.TeamMembers?.Count ?? 0) ?? 0;

            var taskCount = company.Teams?
                .Sum(x => x.TaskItems?.Count ?? 0) ?? 0;

            var completedTaskCount = company.Teams?
                .Sum(x => x.TaskItems?
                    .Count(t => t.Status == Domain.Enums.TaskItemStatus.Done) ?? 0) ?? 0;

            var activeTaskCount = taskCount - completedTaskCount;

            return new CompanyStatisticsDto
            {
                TeamCount = teamCount,
                MemberCount = memberCount,
                ActiveTaskCount = activeTaskCount,
                CompletedTaskCount = completedTaskCount,
                CompletionRate = taskCount == 0
                    ? 0
                    : (double)completedTaskCount / taskCount * 100
            };
        }

        Task<Company?> ICompanyService.GetMyCompanyAsync(Guid ownerId)
        {
            throw new NotImplementedException();
        }
    }
}