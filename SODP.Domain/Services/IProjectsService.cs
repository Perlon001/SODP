using System.Threading.Tasks;
using SODP.Domain.DTO;
using SODP.Model;

namespace SODP.Domain.Services
{
    public interface IProjectsService : IAppService
    {
        Task<ServicePageResponse<ProjectDTO>> GetAllAsync(int currentPage = 1, int pageSize = 10);
        Task<ServiceResponse<ProjectDTO>> GetAsync(int projectId);
        Task<ServiceResponse<ProjectDTO>> CreateAsync(ProjectCreateDTO newProject);
        Task<ServiceResponse> UpdateAsync(ProjectUpdateDTO updateProject);
        Task<ServiceResponse> DeleteAsync(int projectId);
        Task<ServiceResponse> AddBrach(int projectId, int branchId);
    }
}
