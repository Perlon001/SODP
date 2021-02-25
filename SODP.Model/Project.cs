using System.Collections.Generic;
using System.Text.RegularExpressions;
using SODP.Model.Extensions;

namespace SODP.Model
{
    public class Project
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int StageId { get; set; }
        public virtual Stage Stage { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; }
        public string Location { get; set; }
        public ICollection<ProjectBranch> Branches { get; set; }

        public virtual string Symbol 
        {
            get 
            {
                return Number.Trim() + Stage.Sign.Trim();
            }
        }

        public override string ToString()
        {
            return Number.Trim() + Stage.Sign.Trim() + "_" + Title.Trim();
        }

        public void Normalize()
        {
            Regex regex = new Regex("[ ]", RegexOptions.None);
            Title = regex.Replace(Title, "_");
            regex = new Regex("[_]{2,}", RegexOptions.None);
            Title = regex.Replace(Title, "_");
            regex = new Regex("(_+)$", RegexOptions.None);
            Title = regex.Replace(Title, "");
            Title = Title.CapitalizeFirstLetter();
        }
    }
}
