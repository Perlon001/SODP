using SODP.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SODP.Domain.Services
{
    public interface IBranchService : IAppService
    {
        Task<ServicePageResponse<Branch>> GetAllAsync();
        Task<ServiceResponse<Branch>> GetAsync(int branchId);
        Task<ServiceResponse<Branch>> GetAsync(string sign);
        Task<ServiceResponse<Branch>> UpdateAsync(Branch branch);
        Task<ServiceResponse<Branch>> DeleteAsync(Branch branch);
    }
}
