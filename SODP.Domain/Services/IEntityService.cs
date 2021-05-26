using SODP.Domain.DTO;
using SODP.Domain.Models;
using System.Threading.Tasks;

namespace SODP.Domain.Services
{
    public interface IEntityService<T> : IAppService where T : BaseDTO
    {
        Task<ServicePageResponse<T>> GetAllAsync(int currentPage = 1, int pageSize = 0);
        Task<ServiceResponse<T>> GetAsync(int id);
        Task<ServiceResponse<T>> CreateAsync(T entity);
        Task<ServiceResponse> DeleteAsync(int id);
        Task<ServiceResponse> UpdateAsync(T entity);
    }
}
