using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using SODP.Domain.Models;
using SODP.Domain.Services;
using System.Collections.Generic;
using System.Linq;

namespace SODP.Domain.Helpers
{
    public static class ServiceResponseHelper
    {
        public static void ValidationErrorProcess(this ServiceResponse response, ValidationResult validationResult)
        {
            foreach(var item in validationResult.Errors)
            {
                response.ValidationErrors.Add(new KeyValuePair<string, string>(item.PropertyName, item.ErrorMessage));
            }
        }

        public static void IdentityResultErrorProcess(this ServiceResponse response, IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors)
            {
                response.SetError($"{error.Code}: {error.Description}");
            }
        }
    }

}
