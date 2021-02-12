using SODP.Model.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SODP.Model
{
    public class Stage
    {

        public int Id { get; set; }
        public string Sign { get; set; }
        public string Title { get; set; }

        public void Normalize()
        {
            Sign = Sign.ToUpper();
            Title = Title.CapitalizeFirstLetter();
        }
    }
}
