using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SODP.Domain.DTO;
using SODP.Domain.Models;
using SODP.Domain.Services;
using SODP.UI.Pages.Shared;
using SODP.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SODP.UI.Pages.ActiveProjects
{
    [Authorize(Roles = "User, Administrator, ProjectManager")]
    public class IndexModel : SODPPageModel
    {
        private readonly IProjectsService _projectsService;
        private readonly IStagesService _stagesService;

        public IndexModel(IProjectsService projectsService, IStagesService stagesService)
        {
            _projectsService = projectsService;
            _stagesService = stagesService;
            ReturnUrl = "/ActiveProjects";
        }

        public ServicePageResponse<ProjectDTO> Projects { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Projects = await _projectsService.GetAllAsync(0,0);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var response = await _projectsService.DeleteAsync(id);
            if (!response.Success)
            {
                Projects = await _projectsService.GetAllAsync();
                return Page();
            }
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnGetProjectDetailsAsync(int? id)
        {
            IEnumerable<SelectListItem> stages = null;
            var project = (id == null || id == 0) ? new ProjectDTO() : (await _projectsService.GetAsync((int)id)).Data;
            if (id == null)
            {
                var stagesResponse = await _stagesService.GetAllAsync(1, 0);
                stages = stagesResponse.Data.Collection.Select(x => new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = $"({x.Sign.Trim()}) {x.Title.Trim()}"
                }).ToList();
            }
            var viewModel = new NewProjectViewModel { Project = project, Stages = stages };
            var partialViewResult = new PartialViewResult
            {
                ViewName = "_ProjectPartialView",
                ViewData = new ViewDataDictionary<NewProjectViewModel>(ViewData, viewModel)
            }; 
            
            return partialViewResult;
        }

        public async Task<PartialViewResult> OnPostProjectDetails(ProjectDTO project)
        {
            if (ModelState.IsValid)
            {
                var response = (project.Id == 0) ? await _projectsService.CreateAsync(project) : await _projectsService.UpdateAsync(project);
                if (!response.Success)
                {
                    foreach (var error in response.ValidationErrors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                }
            }
            //var partialViewResult = new PartialViewResult(); //await GetPartialViewResult(project.Id);
            var viewModel = new NewProjectViewModel { Project = project, Stages = null };
            var partialViewResult = new PartialViewResult
            {
                ViewName = "_ProjectPartialView",
                ViewData = new ViewDataDictionary<NewProjectViewModel>(ViewData, viewModel)
            };

            return partialViewResult;
        }

        //private async Task<PartialViewResult> GetPartialViewResult(int? id)
        //{
        //    IEnumerable<SelectListItem> stages = null;
        //    var project = (id == null || id == 0) ? new ProjectDTO() : (await _projectsService.GetAsync((int)id)).Data;
        //    if (id == null)
        //    {
        //        var stagesResponse = await _stagesService.GetAllAsync(1, 0);
        //        stages = stagesResponse.Data.Collection.Select(x => new SelectListItem()
        //        {
        //            Value = x.Id.ToString(),
        //            Text = $"({x.Sign.Trim()}) {x.Title.Trim()}"
        //        }).ToList();
        //    }
        //    var viewModel = new NewProjectViewModel { Project = project, Stages = stages };
        //    var partialViewResult = new PartialViewResult
        //    {
        //        ViewName = "_ProjectPartialView",
        //        ViewData = new ViewDataDictionary<NewProjectViewModel>(ViewData, viewModel)
        //    };

        //    return await Task.FromResult(partialViewResult);
        //}
    }
}
