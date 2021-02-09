using FluentValidation;
using SODP.Model;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SODP.Domain.Services;

namespace SODP.Application.Validators
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        private readonly IStagesService _stagesService; 
        public ProjectValidator(IStagesService stageService)
        {
            _stagesService = stageService;

            RuleFor(u => u.Number)
                .NotNull()
                .NotEmpty()
                .WithMessage("Numer jest wymagany")
                .Matches(@"^([1-9]{1})([0-9]{3})$")
                .WithMessage("Numer musi sk³adaæ siê z 4 cyfr.")
                .WithName("Numer");

            RuleFor(u => u.StageSign)
                .NotNull()
                .NotEmpty()
                .WithMessage("Stadium jest wymagane.")
                .MustAsync((sign, cancellation) => StageExist(sign))
                .WithMessage(u => string.Format("Stadium:{0} nie wystêpuje w bazie.", u.StageSign))
                .WithName("Stadium");

            RuleFor(u => u.Title)
                .NotNull()
                .NotEmpty()
                .WithMessage("Tytu³ jest wymagany.")
                .Matches(@"^([a-zA-Z]{1,1})([1-9a-zA-Z_ ]{0,})$")
                .WithMessage("Tytu³ moŸe zawieraæ litery, cyfry, spacjê i podkreœlenie. Pierwszy znaku musi byæ liter¹.")
                .WithName("Tytu³");
        }

        private async Task<bool> StageExist(string sign)
        {
            return await _stagesService.ExistAsync(sign);
        }
    }
}