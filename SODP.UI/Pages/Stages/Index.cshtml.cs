using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SODP.Domain.Services;
using SODP.Model;
using SODP.UI.Pages.Shared;

namespace SODP.UI.Pages.Stages
{
    public class IndexModel : SODPPageModel
    {
        private readonly IStagesService _stagesService;

        public IndexModel(IStagesService stagesService)
        {
            _stagesService = stagesService;
            ReturnUrl = "/Stages";
        }

        [BindProperty]
        public IEnumerable<Stage> Stages { get; set; }

        public async Task OnGet()
        {
            var serviceResponse = await _stagesService.GetAllAsync();
            Stages = serviceResponse.Data.Collection;
        }

        public async Task<IActionResult> OnPostDelete(string sign)
        {
            var response = await _stagesService.DeleteAsync(sign);
            if (!response.Success)
            {
                var serviceResponse = await _stagesService.GetAllAsync();
                Stages = serviceResponse.Data.Collection;
                return Page();
            }

            return RedirectToPage("Index");
        }
    }

}
