using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SODP.Domain.Services;
using SODP.Model;

namespace SODP.UI.Areas.Projects.Pages
{
    [BindProperties]
    public class CreateUpdateModel : PageModel
    {
        private readonly IStagesService _stagesService;
        private readonly IProjectsService _projectsService;

        public CreateUpdateModel(IProjectsService projectsService, IStagesService stagesService)
        {
            _projectsService = projectsService;
            _stagesService = stagesService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IEnumerable<SelectListItem> Stages { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            Input = new InputModel();
            if (id == null)
            {
                var stagesResponse = await _stagesService.GetAllAsync();
                Stages = stagesResponse.Data.Collection.Select(x => new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Title
                }).ToList();

                return Page();
            }

            var response = await _projectsService.GetAsync((int)id);
            if (response == null)
            {
                return NotFound();
            }
            Input.Id = response.Data.Id;
            Input.Number = response.Data.Number;
            Input.StageId = response.Data.StageId;
            Input.StageTitle = response.Data.Stage.Title;
            Input.Title = response.Data.Title;
            Input.Description = response.Data.Description;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            ServiceResponse<Project> response;
            if (ModelState.IsValid)
            {
                var project = new Project
                {
                    Number = Input.Number,
                    StageId = Input.StageId,
                    Title = Input.Title,
                    Description = Input.Description
                };
                if (Input.Id == null)
                {
                    response = await _projectsService.CreateAsync(project);
                }
                else
                {
                    project.Id = (int)Input.Id;
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

        public class InputModel
        {
            public int? Id { get; set; }

            [Required(ErrorMessage ="Numer jest wymagany")]
            [RegularExpression(@"^([1-9]{1})([0-9]{3})$", ErrorMessage = "Numer musi sk³adaæ siê z 4 cyfr.")]
            public string Number { get; set; }
            
            [Required(ErrorMessage ="Stadium jest wymagane")]
            public int StageId { get; set; }

            public string StageTitle { get; set; }

            [Required(ErrorMessage = "Tytu³ jest wymagany")]
            [RegularExpression(@"^([a-zA-Z]{1,1})([1-9a-zA-Z_ ]{0,})$", ErrorMessage = "Tytu³ moŸe zawieraæ litery, cyfry, spacjê i podkreœlenie. Pierwszy znaku musi byæ liter¹.")]
            public string Title { get; set; }

            public string Description { get; set; }

            public IList<SelectListItem> Stages { get; set; }
        }
    }
}
