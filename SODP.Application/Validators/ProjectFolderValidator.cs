using FluentValidation;
using SODP.Model.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Application.Validators
{
    public class ProjectFolderValidator : AbstractValidator<string>
    {
        public ProjectFolderValidator()
        {
            RuleFor(u => u)
                .NotNull()
                .NotEmpty()
                .WithMessage("Nazwa folderu jest wymagana.")
                .Matches(@"^([1-9]{1})([0-9]{3})$")
                .WithMessage("Nazwa foldeu musi rozpoczynac się z 4 cyfr. Pierwsza cyfra nie moze być zerem.");

            RuleFor(u => u.GetUntilOrEmpty("_"))
                .NotNull()
                .NotEmpty()
                .WithMessage("Nazwa folderu musi posiadać separator podkreślenia.");

            //RuleFor(u => u.Remove(0, u.GetUntilOrEmpty("_").Length))
            //    .NotNull()
            //    .NotEmpty()
            //    .WithMessage("Tytuł projektu w nazwie folderu nie może byc pusty.");
        }
    }
}
