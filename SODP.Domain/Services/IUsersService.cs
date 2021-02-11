using SODP.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SODP.Domain.Services
{
    public interface IUsersService : IAppService
    {
        Task<ServicePageResponse<User>> GetAllAsync();
        Task<ServiceResponse<User>> GetAsync(int id);
        Task<ServiceResponse<User>> UpdateAsync(User user);
        Task<ServiceResponse<User>> DeleteAsync(int id);
    }
}
