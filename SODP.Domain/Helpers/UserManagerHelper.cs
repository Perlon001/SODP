using Microsoft.AspNetCore.Identity;
using SODP.Domain.Enums;
using SODP.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SODP.Domain.Helpers
{
    public static class UserManagerHelper
    {
        public static async Task<(IdentityResult, IdentityResult)> UpdateRolesAsync<T>(this UserManager<T> userManager, T user, IList<string> roles) where T : class
        {
            var hasRoles = await userManager.GetRolesAsync(user);
            var removeRoles = new List<string>();
            var addRoles = new List<string>();
            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                if (hasRoles.Contains(role))
                {
                    if (!roles.Contains(role))
                    {
                        removeRoles.Add(role);
                    }
                }
                else
                {
                    if (roles.Contains(role))
                    {
                        addRoles.Add(role);
                    }
                }
            }
            var removeResult = await userManager.RemoveFromRolesAsync(user, removeRoles);
            var addResult = await userManager.AddToRolesAsync(user, addRoles);

            return (removeResult, addResult);
        }
    }
}
