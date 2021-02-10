using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SODP.Domain.Services;
using SODP.Model;

namespace SODP.UI.Areas.Projects.Pages
{
    public class CreateUpdateModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IStagesService _stagesService;
        private readonly IProjectsService _projectsService;

        public CreateUpdateModel(IMapper mapper, IProjectsService projectsService, IStagesService stagesService)
        {
            _mapper = mapper;
            _projectsService = projectsService;
            _stagesService = stagesService;
        }

        [BindProperty]
        public Project Project { get; set; }

        public IList<SelectListItem> Stages { get; set; }

        public string ErrorMessage { get; set; } = "";

        public async Task<IActionResult> OnGet(int? id)
        {
            if(id == null)
            {
                var page = await _stagesService.GetAllAsync();
                Stages = page.Data.Collection.Select(x => new SelectListItem()
                {
                    Text = x.Description,
                    Value = x.Sign
                }).ToList();
                Project = new Project();
                return Page();
            }
            
            var response = await _projectsService.GetAsync(id);
            if(response == null)
            {
                return NotFound();
            }
            Project = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPost(Project project)
        {
            ServiceResponse<Project> response;
            if (ModelState.IsValid)
            {
                if(project.Id == null)
                {
                    response = await _projectsService.CreateAsync(project);
                }
                else
                {
                    response = await _projectsService.UpdateAsync(project);
                }
                
                if (!response.Success)
                {
                    ErrorMessage = response.Message;
                    return RedirectToPage();
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
