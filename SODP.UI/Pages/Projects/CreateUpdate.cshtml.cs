using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SODP.Domain.DTO;
using SODP.Domain.Services;
using SODP.UI.Pages.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SODP.UI.Pages.Projects
{
    [Authorize(Roles = "Administrator,ProjectManager")]
    public class CreateUpdateModel : SODPPageModel
    {
        private readonly IStagesService _stagesService;
        private readonly IMapper _mapper;
        private readonly IProjectsService _projectsService;

        public CreateUpdateModel(IMapper mapper, IProjectsService projectsService, IStagesService stagesService)
        {
            _mapper = mapper;
            _projectsService = projectsService;
            _stagesService = stagesService;
        }

        [BindProperty]
        public ProjectDTO Input { get; set; }

        public IEnumerable<SelectListItem> Stages { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                var stagesResponse = await _stagesService.GetAllAsync(1,10);
                Stages = stagesResponse.Data.Collection.Select(x => new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Title
                }).ToList();

                Input = new ProjectDTO();

                return Page();
            }

            var response = await _projectsService.GetAsync((int)id);
            if (response == null)
            {
                return NotFound();
            }
            Input = response.Data;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                ServiceResponse response;

                if (Input.Id.Equals(0))
                {
                    var project = new ProjectCreateDTO
                    {
                        Number = Input.Number,
                        StageId = Input.Stage.Id,
                        Title = Input.Title,
                        Description = Input.Description
                    };
                    response = await _projectsService.CreateAsync(project);
                }
                else
                {
                    var project = new ProjectUpdateDTO
                    {
                        Id = Input.Id,
                        Number = Input.Number,
                        StageId = Input.Stage.Id,
                        Title = Input.Title,
                        Description = Input.Description
                    }; 
                    response = await _projectsService.UpdateAsync(project);
                }

                if (!response.Success)
                {
                    return Page();
                }

                return RedirectToPage("Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
