using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SODP.Domain.Services;
using SODP.Model;
using System.Threading.Tasks;


namespace SODP.UI.Areas.Projects.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProjectsService _projectsService;

        public IndexModel(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }
        public ServicePageResponse<Project> Projects { get; set; }

        public string ErrorMessage {get; set;} = "";

        public async Task OnGet()
        {
            Projects = await _projectsService.GetAllAsync();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var response = await _projectsService.DeleteAsync(id);
            if (!response.Success)
            {
                Projects = await _projectsService.GetAllAsync();
                ErrorMessage = response.Message;
                return Page();
            }
            return RedirectToPage("Index");
        }
    }
}
