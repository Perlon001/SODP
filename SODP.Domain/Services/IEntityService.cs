using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SODP.Domain.Services
{
    public interface IEntityService<T> : IAppService
    {
        Task<ServicePageResponse<T>> GetAllAsync(int currentPage = 1, int pageSize = 0);
        Task<ServicePageResponse<T>> GetAsync(int id);
        Task<ServiceResponse> DeleteAsync(int id);
    }
}
