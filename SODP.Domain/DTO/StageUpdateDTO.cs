using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace SODP.Domain.DTO
{
    public class StageUpdateDTO : IValidatableObject
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(Title))
            {
                yield return new ValidationResult($"{nameof(Title)} is required.", new[] { nameof(Title) });
            }
        }
    }
}
