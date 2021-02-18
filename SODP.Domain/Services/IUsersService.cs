using SODP.Domain.DTO;
using System.Threading.Tasks;

namespace SODP.Domain.Services
{
    public interface IUsersService : IAppService
    {
        Task<ServicePageResponse<UserDTO>> GetAllAsync();
        Task<ServiceResponse<UserDTO>> GetAsync(int userId);
        Task<ServiceResponse> UpdateAsync(UserUpdateDTO user);
        Task<ServiceResponse> DeleteAsync(int userId);
        Task<ServicePageResponse<string>> GetRolesAsync(int userId);
    }
}
