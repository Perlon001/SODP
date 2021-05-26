using System.Threading.Tasks;
using SODP.Domain.DTO;
using SODP.Domain.Models;

namespace SODP.Domain.Services
{
    public interface IStagesService : IEntityService<StageDTO>
    {
        Task<ServiceResponse<StageDTO>> GetAsync(string sign);
        Task<ServiceResponse> DeleteAsync(string sign);
        Task<bool> ExistAsync(int id);
        Task<bool> ExistAsync(string sign);
    }
}
