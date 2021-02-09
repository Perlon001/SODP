using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SODP.Model;
using SODP.Domain.Services;

namespace SODP.UI.Areas.Stages.Pages
{
    public class EditModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IStagesService _stagesService;

        public EditModel(IMapper mapper, IStagesService stagesService)
        {
            _mapper = mapper;
            _stagesService = stagesService;
        }

        [BindProperty]
        public Stage Stage { get; set; }

        public string ErrorMessage { get; set; } = "";

        public async Task OnGet(string sign)
        {
            var response = await _stagesService.GetAsync(sign);
            Stage = response.Data;
        }

        public async Task<IActionResult> OnPost(Stage stage)
        {
            if (ModelState.IsValid)
            {
                var response = await _stagesService.UpdateAsync(stage);
                if (!response.Success)
                {
                    ErrorMessage = response.Message;
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
