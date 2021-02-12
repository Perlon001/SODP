using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SODP.Domain.Services;
using SODP.Model;
using SODP.UI.Pages.Shared;
using System.Threading.Tasks;


namespace SODP.UI.Areas.Stages.Pages
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
        public ServicePageResponse<Stage> Stages { get; set; }

        public async Task OnGet()
        {
            Stages = await _stagesService.GetAllAsync();
        }

        public async Task<IActionResult> OnPostDelete(string sign)
        {
            var response = await _stagesService.DeleteAsync(sign);
            if (!response.Success)
            {
                Stages = await _stagesService.GetAllAsync();
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
