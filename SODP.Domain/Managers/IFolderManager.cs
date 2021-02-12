using SODP.Model;
using System.Threading.Tasks;

namespace SODP.Domain.Managers
{
    public interface IFolderManager
    {
        Task<(string Command, bool Success)> CreateOrUpdateFolderAsync(Project project);
        Task<(string Command ,bool Success)> DeleteFolderAsync(Project project);
    }
}
