using SODP.Model;
using System.Threading.Tasks;

namespace SODP.Domain.Managers
{
    public interface IFolderManager
    {
        Task<(string Command, bool Success)> CreateOrUpdateFolder(Project project);
        Task<(string Command ,bool Success)> DeleteFolder(Project project);
    }
}
