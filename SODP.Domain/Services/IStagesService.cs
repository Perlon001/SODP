using System.Threading.Tasks;
using SODP.Domain.DTO;
using SODP.Model;

namespace SODP.Domain.Services
{
    public interface IStagesService : IAppService
    {
        Task<ServicePageResponse<StageDTO>> GetAllAsync(int currentPage, int pageSize);
        Task<ServiceResponse<StageDTO>> GetAsync(int id);
        Task<ServiceResponse<StageDTO>> GetAsync(string sign);
        Task<ServiceResponse<StageDTO>> CreateAsync(StageDTO stage);
        Task<ServiceResponse> UpdateAsync(StageDTO stage);
        Task<ServiceResponse> DeleteAsync(int id);
        Task<ServiceResponse> DeleteAsync(string sign);
        Task<bool> ExistAsync(int id);
        Task<bool> ExistAsync(string sign);
    }
}
