using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SODP.Domain.Services;
using SODP.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SODP.Application.Services
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<Role> _rolesManager;

        public RolesService(RoleManager<Role> rolesManager)
        {
            _rolesManager = rolesManager;
        }

        public async Task<IList<string>> GetAllAsync() => await _rolesManager.Roles.Select(x => x.Name).ToListAsync();
    }
}
