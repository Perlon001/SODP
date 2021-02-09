using System.Threading.Tasks;
using SODP.Model;

namespace SODP.Domain.Services
{
    public interface IProjectsService : IAppService
    {
        Task<ServicePageResponse<Project>> GetAllAsync(int page_number = 0, int page_size = 0);
        Task<ServiceResponse<Project>> GetAsync(int id);
        Task<ServiceResponse<Project>> CreateAsync(Project newProject);
        Task<ServiceResponse<Project>> UpdateAsync(Project updateProject);
        Task<ServiceResponse<Project>> DeleteAsync(int id);
    }
}
