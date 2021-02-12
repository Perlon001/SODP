using FluentValidation;
using SODP.Model;

namespace SODP.Application.Validators
{
    public class StageValidator : AbstractValidator<Stage>
    {
        public StageValidator()
        {
            RuleFor(x => x.Sign)
                .NotNull()
                .NotEmpty()
                .WithMessage("Ozneczenie stadium jest wymagane.")
                .Matches(@"^([a-zA-Z]{2})([a-zA-Z _]{0,})$")
                .WithMessage("Znak moze zawierać litery i podkreślenie. Na początku minimum 2 litery")
                .WithName("Znak");

            RuleFor(x => x.Title)
                .NotNull()
                .NotEmpty()
                .WithMessage("Opis stadium jest wymagany.")
                .MaximumLength(10)
                .WithMessage("Maksimum 10 znaków.")
                .WithName("Opis");
        }
    }
}
