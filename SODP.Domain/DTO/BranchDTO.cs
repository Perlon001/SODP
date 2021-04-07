using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Domain.DTO
{
    public class BranchDTO
    {
        public int Id { get; set; }
        public string Sign { get; set; }
        public string Name { get; set; }
        public int DesignLicenceId { get; set; }
        public int CheckingLicenceId { get; set; }
    }
}
