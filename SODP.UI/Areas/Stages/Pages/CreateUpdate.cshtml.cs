using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly IStagesService _stagesService;

        public CreateUpdateModel(IStagesService stagesService)
        {
            _stagesService = stagesService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ErrorMessage { get; set; } = "";

        public async Task<IActionResult> OnGet(int? id)
        {
            Input = new InputModel();
            if (id != null)
            {
                var response = await _stagesService.GetAsync((int)id);
                if (!response.Success)
                {
                    return NotFound();
                }
                Input.Id = (int)id;
                Input.Sign = response.Data.Sign;
                Input.Title = response.Data.Title;
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var stage = new Stage
                {
                    Id = Input.Id,
                    Sign = Input.Sign,
                    Title = Input.Title
                };

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

        public class InputModel
        {
            public int Id { get; set; }
            
            [Required(ErrorMessage = "Ozneczenie stadium jest wymagane.")]
            [RegularExpression(@"^([a-zA-Z]{2})([a-zA-Z _]{0,})$", ErrorMessage = "Znak moze zawieraæ litery i podkreœlenie. Na pocz¹tku minimum 2 litery")]
            public string Sign { get; set; }

            [Required(ErrorMessage ="Tytu³ stadium jest wymagany.")]
            public string Title { get; set; }


        }
    }
}
