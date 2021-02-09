using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SODP.Domain.Services;
using SODP.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SODP.UI.Areas.Projects.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IStagesService _stagesService;
        private readonly IProjectsService _projectsService;

        public CreateModel(IMapper mapper, IProjectsService projectsService, IStagesService stagesService)
        {
            _mapper = mapper;
            _projectsService = projectsService;
            _stagesService = stagesService;
        }
        [BindProperty]
        public Project Project { get; set; }

        public IList<SelectListItem> Stages { get; set; }

        public string ErrorMessage { get; set; } = "";

        public async Task OnGet()
        {
            Stages = await GetStages();
        }

        public async Task<IActionResult> OnPost(Project project)
        {
            if (ModelState.IsValid)
            {
                var response = await _projectsService.CreateAsync(project);
                if (!response.Success)
                {
                    Stages = await GetStages();
                    ErrorMessage = response.Message;
                    return Page();
                }
                return RedirectToPage("Index");
            }
            Stages = await GetStages();
            return Page();
        }

        private async Task<IList<SelectListItem>> GetStages()
        {
            var page = await _stagesService.GetAllAsync();
            return page.Data.Collection.Select(x => new SelectListItem()
            {
                Text = x.Description,
                Value = x.Sign
            }).ToList();
        }
    }
}
