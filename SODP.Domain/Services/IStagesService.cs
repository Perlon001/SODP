using System.Threading.Tasks;
using SODP.Model;

namespace SODP.Domain.Services
{
    public interface IStagesService : IAppService
    {
        Task<ServicePageResponse<Stage>> GetAllAsync();
        Task<ServiceResponse<Stage>> GetAsync(string sign);
        Task<ServiceResponse<Stage>> CreateAsync(Stage stage);
        Task<ServiceResponse<Stage>> UpdateAsync(Stage stage);
        Task<ServiceResponse<Stage>> DeleteAsync(string sign);
        Task<bool> ExistAsync(string sign);
    }
}
