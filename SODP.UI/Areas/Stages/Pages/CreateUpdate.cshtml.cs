using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SODP.Domain.Services;
using SODP.Model;

namespace SODP.UI.Areas.Stages.Pages
{
    public class CreateUpdateModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IStagesService _stagesService;

        public CreateUpdateModel(IMapper mapper, IStagesService stagesService)
        {
            _mapper = mapper;
            _stagesService = stagesService;
        }

        [BindProperty]
        public Stage Stage { get; set; }

        public string ErrorMessage { get; set; } = "";

        public async Task<IActionResult> OnGet(string sign)
        {
            if(sign == null)
            {
                Stage = new Stage();
                return Page();
            }
            var response = await _stagesService.GetAsync(sign);
            if (!response.Success)
            {
                return NotFound();
            }
            Stage = response.Data;
            return Page();
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
