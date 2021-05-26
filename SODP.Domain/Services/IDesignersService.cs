using SODP.Domain.DTO;
using SODP.Domain.Models;
using System.Threading.Tasks;

namespace SODP.Domain.Services
{
    public interface IDesignersService : IEntityService<DesignerDTO>
    {
        Task<ServicePageResponse<DesignerDTO>> GetAllAsync(int currentPage = 1, int pageSize = 0, bool active = false);
    }
}
