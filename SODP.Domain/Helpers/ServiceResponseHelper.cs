using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using SODP.Domain.Services;
using System.Linq;

namespace SODP.Domain.Helpers
{
    public static class ServiceResponseHelper
    {
        public static void ValidationErrorProcess(this ServiceResponse response, ValidationResult validationResult)
        {
            response.ValidationErrors = validationResult.Errors.Select(x => string.Format("{0}: {1}", x.PropertyName, x.ErrorMessage)).ToList();
        }

        public static void IdentityResultErrorProcess(this ServiceResponse response, IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors)
            {
                response.SetError(string.Format("{0}: {1}", error.Code, error.Description));
            }
        }
    }

}
