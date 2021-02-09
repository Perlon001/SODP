using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SODP.Domain.Services;
using SODP.Model;
using System.Threading.Tasks;

namespace SODP.UI.Areas.Stages.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IStagesService _stagesService;

        public CreateModel(IMapper mapper, IStagesService stagesService)
        {
            _mapper = mapper;
            _stagesService = stagesService;
        }

        [BindProperty]
        public Stage Stage { get; set; }

        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
        }
        
        //public class StageUI
        //{
        //    public string Sign { get; set; }
        //    public string Description { get; set; }
        //}

        //public class StageValidator : AbstractValidator<Stage>
        //{
        //    public StageValidator()
        //    {
        //        RuleFor(x => x.Sign).NotNull().NotEmpty().WithName("Znak").Matches("^([a-zA-Z]{2})").WithMessage("Na pocz¹tku minimum 2 litery").Matches("^([a-zA-Z]{2})([a-zA-Z _]{0,})$").WithMessage("Znak moze zawieraæ litery i podkreœlenie");
        //        RuleFor(x => x.Description).NotNull().NotEmpty().WithName("Opis");
        //    }
        //}

        public async Task<IActionResult> OnPost(Stage stage)
        {
            if (ModelState.IsValid)
            {
                var response = await _stagesService.CreateAsync(_mapper.Map<Stage>(stage));
                if (!response.Success)
                {
                    ErrorMessage = response.Message;
                    return Page();
                }
                return RedirectToPage("Index");
            }
            return Page();

            //var validator = new StageValidator();
            //var result = validator.Validate(stage);
            //result.AddToModelState(ModelState,"Stage");

            //if (result.IsValid)
            //{
            //    
            //    if (!response.Success)
            //    {
            //        ErrorMessage = response.Message;
            //        return RedirectToPage();
            //    }
            //    return RedirectToPage("Index");
            //}

            //return Page();
        }
    }
}
