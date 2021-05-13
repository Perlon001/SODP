using System.Threading.Tasks;
using SODP.Domain.DTO;
using SODP.Model;
using SODP.Model.Enums;

namespace SODP.Domain.Services
{
    public interface IProjectsService : IAppService
    {
        IProjectsService SetArchiveMode();
        Task<ServicePageResponse<ProjectDTO>> GetAllAsync(int currentPage = 1, int pageSize = 0);
        Task<ServiceResponse<ProjectDTO>> GetAsync(int projectId);
        Task<ServiceResponse<ProjectDTO>> CreateAsync(ProjectDTO newProject);
        Task<ServiceResponse> UpdateAsync(ProjectDTO updateProject);
        Task<ServiceResponse> DeleteAsync(int projectId);
        Task<ServiceResponse> RestoreAsync(int projectId);
        Task<ServiceResponse> ArchiveAsync(int projectId);
        Task<ServicePageResponse<BranchDTO>> GetBranchesAsync(int projectId);
        Task<ServiceResponse> AddBranchAsync(int projectId, int branchId);
        Task<ServiceResponse> DeleteBranchAsync(int projectId, int branchId);

    }
}
