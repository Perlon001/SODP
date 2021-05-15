using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Domain.DTO
{
    public class BranchDTO  : BaseDTO
    {
        public string Sign { get; set; }
        public string Name { get; set; }
        public DesignerDTO DesignLicenceId { get; set; }
        public DesignerDTO CheckingLicenceId { get; set; }
    }
}
