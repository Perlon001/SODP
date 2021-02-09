using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SODP.Model;
using SODP.Domain.Services;

namespace SODP.UI.Areas.Projects.Pages
{
    public class EditModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IStagesService _stagesService;
        private readonly IProjectsService _projectsService;

        public EditModel(IMapper mapper, IProjectsService projectsService, IStagesService stagesService)
        {
            _mapper = mapper;
            _projectsService = projectsService;
            _stagesService = stagesService;
        }

        [BindProperty]
        public Project Project { get; set; }

        public IList<SelectListItem> Stages { get; set; }

        public string ErrorMessage {get; set;} = "";

        public async Task OnGet(int id)
        {
            var page = await _stagesService.GetAllAsync();
            Stages = page.Data.Collection.Select(x => new SelectListItem()
            {
                Text = x.Description,
                Value = x.Sign
            }).ToList();

            var response = await _projectsService.GetAsync(id);
            Project = response.Data;
        }

        public async Task<IActionResult> OnPost(Project project)
        {
            if (ModelState.IsValid)
            {
                var response = await _projectsService.UpdateAsync(project);
                if (!response.Success)
                {
                    ErrorMessage = response.Message;
                    return Page();
                }
                return RedirectToPage("Index");
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
