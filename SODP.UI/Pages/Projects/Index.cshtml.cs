using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SODP.Domain.DTO;
using SODP.Domain.Services;
using SODP.UI.Pages.Shared;
using System;
using System.Threading.Tasks;

namespace SODP.UI.Pages.Projects
{
    [Authorize(Roles = "User, Administrator, ProjectManager")]
    public class IndexModel : SODPPageModel
    {
        private readonly IProjectsService _projectsService;

        public IndexModel(IProjectsService projectsService)
        {
            _projectsService = projectsService;
            ReturnUrl = "/Projects";
        }
        public ServicePageResponse<ProjectDTO> Projects { get; set; }


        public async Task OnGet()
        {
            Projects = await _projectsService.GetAllAsync(0,0);
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var response = await _projectsService.DeleteAsync(id);
            if (!response.Success)
            {
                Projects = await _projectsService.GetAllAsync();
                return Page();
            }
            return RedirectToPage("Index");
        }
    }
}
