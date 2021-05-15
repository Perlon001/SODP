using System.ComponentModel.DataAnnotations;

namespace SODP.Domain.DTO
{
    public class StageDTO  : BaseDTO
    {
        [Required]
        [MinLength(2)]
        public string Sign { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
