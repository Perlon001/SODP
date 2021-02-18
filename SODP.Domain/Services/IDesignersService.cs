using SODP.Domain.DTO;
using SODP.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SODP.Domain.Services
{
    public interface IDesignersService : IAppService
    {
        Task<ServicePageResponse<Designer>> GetAllAsync();
        Task<ServiceResponse<Designer>> GetAsync(int designerId);
        Task<ServiceResponse<Designer>> CreateAsync(DesignerCreateDTO designer);
        Task<ServiceResponse<Designer>> UpdateAsync(DesignerUpdateDTO designer);
        Task<ServiceResponse> DeleteAsync(int designerId);


    }
}
