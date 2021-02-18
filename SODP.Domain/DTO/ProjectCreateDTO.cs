using System.ComponentModel.DataAnnotations;

namespace SODP.Domain.DTO
{
    public class ProjectCreateDTO
    {
        [Required(ErrorMessage = "{Property} is required.")]
        [RegularExpression(@"^([1-9]{1})([0-9]{3})$", ErrorMessage = "{Property} should contain 4 digits.")]
        public string Number { get; set; }

        [Required(ErrorMessage = "{Property} is required.")]
        public int StageId { get; set; }

        [Required(ErrorMessage = "{Property} is required.")]
        [RegularExpression(@"^([a-zA-Z]{1,1})([1-9a-zA-Z_ ]{0,})$", ErrorMessage = "{Property} must start with a letter, contain letters without Polish characters, numbers and an underscore")]
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
