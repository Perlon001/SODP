using SODP.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests.DataAccess
{
    public class ProjectFolderValidatorTests
    {
        private readonly ProjectNameValidator _validator;
        public ProjectFolderValidatorTests()
        {
            _validator = new ProjectNameValidator();
        }

        [Fact]
        public void when_project_not_have_first_four_digit_should_return_false()
        {
            var projectName = @"123abc_azxcvb";
            Assert.False(_validator.Validate(projectName));
        }
    }
}
