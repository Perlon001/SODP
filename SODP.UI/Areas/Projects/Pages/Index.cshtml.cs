using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SODP.Domain.Services;
using SODP.Model;
using SODP.UI.Pages.Shared;
using System.Threading.Tasks;


namespace SODP.UI.Areas.Projects.Pages
{
    public class IndexModel : SODPPageModel
    {
        private readonly IProjectsService _projectsService;

        public IndexModel(IProjectsService projectsService)
        {
            _projectsService = projectsService;
            ReturnUrl = "/Projects";
        }
        public ServicePageResponse<Project> Projects { get; set; }

        public void OnGet()
        {
            //Projects = await _projectsService.GetAllAsync();
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
