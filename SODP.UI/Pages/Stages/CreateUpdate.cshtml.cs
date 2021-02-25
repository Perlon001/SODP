using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SODP.Domain.DTO;
using SODP.Domain.Services;
using SODP.UI.Pages.Shared;

namespace SODP.UI.Pages.Stages
{
    [Authorize(Roles = "Administrator, ProjectManager")]
    public class CreateUpdateModel : SODPPageModel
    {
        private readonly IStagesService _stagesService;

        public CreateUpdateModel(IStagesService stagesService)
        {
            _stagesService = stagesService;
            ReturnUrl = "/Stages";
        }

        [BindProperty]
        public StageDTO Input { get; set; }

        public string ErrorMessage { get; set; } = "";

        public async Task<IActionResult> OnGet(int? id)
        {
            Input = new StageDTO();
            if (id != null)
            {
                var response = await _stagesService.GetAsync((int)id);
                if (!response.Success)
                {
                    return NotFound();
                }
                Input = response.Data;
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
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
                    foreach(var message in response.ValidationErrors)
                    {
                        ErrorMessage += message;
                    }
                    return Page();
                }

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
