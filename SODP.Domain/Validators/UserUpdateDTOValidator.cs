using FluentValidation;
using FluentValidation.Results;
using SODP.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Domain.Validators
{
    public class UserUpdateDTOValidator : AbstractValidator<UserUpdateDTO>
    {
        public UserUpdateDTOValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .WithMessage("Id jest wymagany.");
        }
    }
}
