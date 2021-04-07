using SODP.Model;
using System.Threading.Tasks;

namespace SODP.Domain.Managers
{
    public interface IFolderManager
    {
        Task<(string Command, bool Success)> CreateFolderAsync(Project project);
        Task<(string Command, bool Success)> RenameFolderAsync(Project project);
        Task<(string Command, bool Success)> ArchiveFolderAsync(Project project);
        Task<(string Command ,bool Success)> DeleteFolderAsync(Project project);
    }
}
