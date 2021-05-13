using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SODP.Domain.DTO;
using SODP.Domain.Services;
using SODP.UI.Pages.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SODP.UI.Pages.ActiveProjects
{
    [Authorize(Roles = "Administrator,ProjectManager")]
    public class CreateUpdateModel : SODPPageModel
    {
        private readonly IStagesService _stagesService;
        private readonly IProjectsService _projectsService;

        public CreateUpdateModel(IProjectsService projectsService, IStagesService stagesService)
        {
            _projectsService = projectsService;
            _stagesService = stagesService;
        }

        [BindProperty]
        public ProjectDTO Project { get; set; }

        public IEnumerable<SelectListItem> Stages { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                var stagesResponse = await _stagesService.GetAllAsync(1,0);
                Stages = stagesResponse.Data.Collection.Select(x => new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = String.Format("({0}) {1}", x.Sign.Trim(), x.Title.Trim())
                }).ToList();

                Project = new ProjectDTO();

                return Page();
            }

            var response = await _projectsService.GetAsync((int)id);
            if (response == null)
            {
                return NotFound();
            }
            Project = response.Data;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var stagesResponse = await _stagesService.GetAllAsync(1, 0);
            Stages = stagesResponse.Data.Collection.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = String.Format("({0}) {1}", x.Sign.Trim(), x.Title.Trim())
            }).ToList();

            if (ModelState.IsValid)
            {
                ServiceResponse response;

                if (Project.Id.Equals(0))
                {
                    var project = new ProjectDTO
                    {
                        Number = Project.Number,
                        StageId = Project.StageId,
                        Title = Project.Title,
                        Description = Project.Description
                    };
                    response = await _projectsService.CreateAsync(project);
                }
                else
                {
                    var project = new ProjectDTO
                    {
                        Id = Project.Id,
                        Number = Project.Number,
                        StageId = Project.StageId,
                        Title = Project.Title,
                        Description = Project.Description
                    };
                    response = await _projectsService.UpdateAsync(project);
                }

                if (!response.Success)
                {
                    return Page();
                }

                return RedirectToPage("Index");
            }
          
            return Page();
        }
    }
}
