using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SODP.Domain.DTO;
using SODP.Domain.Services;
using SODP.UI.Pages.Shared;

namespace SODP.UI.Pages.Projects
{
    [Authorize(Roles = "Administrator,ProjectManager")]
    public class InfoModel : SODPPageModel
    {
        private readonly IProjectsService _projectsService;

        public InfoModel(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        public ProjectDTO Project { get; set; }
        public IList<string> Branches { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _projectsService.GetAsync(id);
            Project = response.Data;

            return Page();
        }
    }
}
