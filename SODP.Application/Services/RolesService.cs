using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using SODP.DataAccess;
using SODP.Domain.Services;
using SODP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public IList<string> GetAll() => _rolesManager.Roles.Select(x => x.Name).ToList();
    }
}
