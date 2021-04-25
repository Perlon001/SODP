using Microsoft.AspNetCore.Identity;
using SODP.Model;
using System;
using System.Threading.Tasks;

namespace SODP.DataAccess
{
    public class UserInitializer : IDisposable
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserInitializer(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void UserInit()
        {
            CreateRoleIfNotExist(_roleManager, "Administrator").Wait();
            CreateRoleIfNotExist(_roleManager, "User").Wait();
            CreateRoleIfNotExist(_roleManager, "ProjectManager").Wait();

            CreateUserIfNotExist(_userManager, "Administrator", "Administrator").Wait();
            AddToRoleIfNotExist(_userManager, "Administrator", "Administrator").Wait();

        }

        static async Task<bool> CreateRoleIfNotExist(RoleManager<Role> roleManager, string role)
        {
            var result = await roleManager.RoleExistsAsync(role);

            if (!result)
            {
                var roleResult = await roleManager.CreateAsync(new Role(role));
                result = roleResult.Succeeded;
            }

            return result;
        }

        static async Task<bool> CreateUserIfNotExist(UserManager<User> userManager, string userName, string password)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                var result = await userManager.CreateAsync(new User(userName), password);
                return result.Succeeded;
            }

            return true;
        }

        static async Task<bool> AddToRoleIfNotExist(UserManager<User> userManager, string userName, string role)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (!(await userManager.IsInRoleAsync(user, "Administrator")))
            {
                var result = await userManager.AddToRoleAsync(user, role);
                return result.Succeeded;
            }

            return true;
        }

        public void Dispose()
        {
        }
    }
}
