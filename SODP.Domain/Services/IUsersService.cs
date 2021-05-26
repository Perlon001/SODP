using SODP.Domain.DTO;
using SODP.Domain.Models;
using System.Threading.Tasks;

namespace SODP.Domain.Services
{
    public interface IUsersService : IEntityService<UserDTO>
    {
        Task<ServicePageResponse<UserDTO>> GetAllAsync();
        Task<ServicePageResponse<RoleDTO>> GetRolesAsync(int userId);
    }
}
