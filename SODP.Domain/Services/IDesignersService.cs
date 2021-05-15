using SODP.Domain.DTO;
using SODP.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SODP.Domain.Services
{
    public interface IDesignersService : IEntityService<DesignerDTO>
    {
        Task<ServicePageResponse<DesignerDTO>> GetAllAsync(int currentPage = 1, int pageSize = 0, bool active = false);
        Task<ServiceResponse<DesignerDTO>> CreateAsync(DesignerDTO designer);
        Task<ServiceResponse<DesignerDTO>> UpdateAsync(DesignerDTO designer);


    }
}
