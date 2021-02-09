using SODP.Model.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SODP.Model
{
    public class Stage
    {
        public string Sign { get; set; }
        public string Description { get; set; }

        public void Normalize()
        {
            Sign = Sign.ToUpper();
            Description = Description.CapitalizeFirstLetter();
        }
    }
}
