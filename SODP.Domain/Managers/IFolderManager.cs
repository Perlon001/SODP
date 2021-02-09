using SODP.Model;
using System.Threading.Tasks;

namespace SODP.Domain.Managers
{
    public interface IFolderManager
    {
        Task<(string,bool)> CreateOrUpdateFolder(Project project);
        Task<(string,bool)> DeleteFolder(Project project);
    }
}
