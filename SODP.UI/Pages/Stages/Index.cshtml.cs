using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> OnGet(int currentPage = 1, int pageSize = 10)
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

        public async Task<IActionResult> OnPostDelete(string sign)
        {
            var response = await _stagesService.DeleteAsync(sign);
            if (!response.Success)
            {
                return Page();
            }

            return RedirectToPage("Index");
        }

        private async Task<IList<StageDTO>> GetStages(PageInfo pageInfo)
        {
            var serviceResponse = await _stagesService.GetAllAsync(pageInfo.CurrentPage, pageInfo.ItemsPerPage);
            pageInfo.TotalItems = serviceResponse.Data.TotalCount;

            return serviceResponse.Data.Collection.ToList();
        }
    }

}
