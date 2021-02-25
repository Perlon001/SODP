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

        [BindProperty]
        public StageDTO Input { get; set; } = new StageDTO();

        [BindProperty]
        public bool IsModalShown { get; set; }


        public async Task<IActionResult> OnGetAsync(int currentPage = 1, int pageSize = 10)
        {
            var url = new StringBuilder();
            url.Append("/Stages?currentPage=:");

            StagesViewModel = new StagesViewModel
            {
                PageInfo = new PageInfo
                {
                    CurrentPage = currentPage,
                    ItemsPerPage = pageSize,
                    Url = url.ToString()
                },
            };
            StagesViewModel.Stages = await GetStages(StagesViewModel.PageInfo);

            return Page();
        }

        public PartialViewResult OnGetStageModalPartial()
        {
            return new PartialViewResult()
            {
                ViewName = "_StageCreatePartialView",
                ViewData = new ViewDataDictionary<StageDTO>(ViewData, new StageDTO())
            };
        }

        public async Task<IActionResult> OnPostAsync(bool IsModalShown)
        {
            ServiceResponse response;
            if (ModelState.IsValid)
            {

                if (Input.Id.Equals(0))
                {
                    var stage = new StageCreateDTO
                    {
                        Sign = Input.Sign,
                        Title = Input.Title
                    };
                    response = await _stagesService.CreateAsync(stage);
                }
                else
                {
                    var stage = new StageUpdateDTO
                    {
                        Id = Input.Id,
                        Title = Input.Title
                    };
                    response = await _stagesService.UpdateAsync(stage);
                }
                if (response.ValidationErrors.Count > 0)
                {
                    foreach (var message in response.ValidationErrors)
                    {
                    }
                    return Page();
                }

                if (!response.Success)
                {
                    await OnGetAsync();
                    return Page();
                }
                return RedirectToPage("Index");
            }
            else
            {
                await OnGetAsync();
                return Page();
            }
        }

        private async Task<IList<StageDTO>> GetStages(PageInfo pageInfo)
        {
            var serviceResponse = await _stagesService.GetAllAsync(pageInfo.CurrentPage, pageInfo.ItemsPerPage);
            pageInfo.TotalItems = serviceResponse.Data.TotalCount;

            return serviceResponse.Data.Collection.ToList();
        }
    }

}
