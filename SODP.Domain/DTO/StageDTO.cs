using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SODP.Domain.DTO
{
    public class StageDTO : IValidatableObject
    {
        public string Sign { get; set; }

        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var rxSign = new Regex(@"^([a-zA-Z]{2})([a-zA-Z0-9]{0,})$");
            if (String.IsNullOrEmpty(Sign))
            {
                yield return new ValidationResult($"{nameof(Sign)} is required.", new[] { nameof(Sign) });
            }
            if (!rxSign.IsMatch(Sign))
            {
                yield return new ValidationResult($"{nameof(Sign)} should at least 2 letters at the beginning.", new[] { nameof(Sign) });
            }
            if (String.IsNullOrEmpty(Description))
            {
                yield return new ValidationResult($"{nameof(Description)} is required.", new[] { nameof(Description) });
            }
        }
    }
}
