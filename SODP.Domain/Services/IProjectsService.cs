using SODP.Domain.DTO;
using SODP.Domain.Models;
using System.Threading.Tasks;

namespace SODP.Domain.Services
{
    public interface IProjectsService : IEntityService<ProjectDTO>
    {
        IProjectsService SetArchiveMode();
        Task<ServiceResponse> RestoreAsync(int id);
        Task<ServiceResponse> ArchiveAsync(int id);
        Task<ServicePageResponse<BranchDTO>> GetBranchesAsync(int id);
        Task<ServiceResponse> AddBranchAsync(int projectId, int branchId);
        Task<ServiceResponse> DeleteBranchAsync(int projectId, int branchId);

    }
}
