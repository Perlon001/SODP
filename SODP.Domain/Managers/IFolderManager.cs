using SODP.Model;
using SODP.Model.Enums;
using System.Threading.Tasks;

namespace SODP.Domain.Managers
{
    public interface IFolderManager
    {
       Task<(bool Success, string Message)> CreateFolderAsync(Project project);
        Task<(bool Success, string Message)> RenameFolderAsync(Project project);
        Task<(bool Success, string Message)> ArchiveFolderAsync(Project project);
        Task<(bool Success, string Message)> RestoreFolderAsync(Project project);
        Task<(bool Success, string Message)> DeleteFolderAsync(Project project);
    }
}
