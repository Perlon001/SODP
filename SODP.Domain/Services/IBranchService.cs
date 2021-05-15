using SODP.Domain.DTO;
using SODP.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SODP.Domain.Services
{
    public interface IBranchService : IAppService
    {
        Task<ServicePageResponse<BranchDTO>> GetAllAsync();
        Task<ServiceResponse<BranchDTO>> GetAsync(int branchId);
        Task<ServiceResponse<BranchDTO>> GetAsync(string sign);
        Task<ServiceResponse<BranchDTO>> CreateAsync(BranchDTO newBranch);
        Task<ServiceResponse<Branch>> UpdateAsync(BranchDTO updateBranch);
        Task<ServiceResponse> DeleteAsync(int branchId);
    }
}
