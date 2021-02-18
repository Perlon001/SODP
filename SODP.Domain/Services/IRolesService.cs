using System.Collections.Generic;
using System.Threading.Tasks;

namespace SODP.Domain.Services
{
    public interface IRolesService : IAppService
    {
        IList<string> GetAll();
    }
}
