using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Model
{
    public class ProjectBranch
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
        public int? DesignerLicenceId { get; set; }
        public virtual Licence DesignerLicence { get; set; }
        public int? CheckingLicenceId { get; set; }
        public virtual Licence CheckingLicence { get; set; }
    }
}
