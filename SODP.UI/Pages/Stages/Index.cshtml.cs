using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SODP.Domain.DTO;
using SODP.Domain.Model;
using SODP.Domain.Services;
using SODP.UI.Pages.Shared;
using SODP.UI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SODP.UI.Pages.Stages
{
    [Authorize(Roles = "Administrator, ProjectManager")]
    public class IndexModel : SODPPageModel
    {
        private readonly IStagesService _stagesService;

        public IndexModel(IStagesService stagesService)
        {
            _stagesService = stagesService;
            ReturnUrl = "/Stages";
        }

        [BindProperty]
        public StagesViewModel StagesViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int currentPage = 1, int pageSize = 15)
        {
            var url = new StringBuilder();
            url.Append("/Stages?currentPage=:&pageSize=");
            url.Append(pageSize.ToString());

            StagesViewModel = new StagesViewModel
            {
                PageInfo = new PageInfo
                {
                    CurrentPage = currentPage,
                    ItemsPerPage = pageSize,
                    Url = url.ToString()
                },
            };
            StagesViewModel.Stages = await GetStagesAsync(StagesViewModel.PageInfo);

            return Page();
        }


        public async Task<PartialViewResult> OnGetStageDetailsAsync(int? id)
        {
            StageDTO stage;
            if (id != null)
            {
                var response = await _stagesService.GetAsync((int)id);
                stage = response.Data;
            }
            else
            {
                stage = new StageDTO();
            }
            var partialViewResult = new PartialViewResult()
            {
                ViewName = "_StagePartialView",
                ViewData = new ViewDataDictionary<StageDTO>(ViewData, stage)
            };

            return await Task.FromResult(partialViewResult);
        }

        public async Task<PartialViewResult> OnPostStageDetailsAsync(StageDTO stage)
        {
            ServiceResponse response ;
            if (ModelState.IsValid)
            {
                if (stage.Id == 0)
                {
                    response = await _stagesService.CreateAsync(stage);
                }
                else
                {
                    response = await _stagesService.UpdateAsync(stage);
                }
                if (!response.Success)
                {
                    foreach(var error in response.ValidationErrors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                }
            }

            var partialViewResult = new PartialViewResult()
            {
                ViewName = "_StagePartialView",
                ViewData = new ViewDataDictionary<StageDTO>(ViewData, stage)
            };

            return partialViewResult;
        }

        private async Task<IList<StageDTO>> GetStagesAsync(PageInfo pageInfo)
        {
            var serviceResponse = await _stagesService.GetAllAsync(pageInfo.CurrentPage, pageInfo.ItemsPerPage);
            pageInfo.TotalItems = serviceResponse.Data.TotalCount;
            pageInfo.CurrentPage = serviceResponse.Data.PageNumber;

            return serviceResponse.Data.Collection.ToList();
        }
    }
}
