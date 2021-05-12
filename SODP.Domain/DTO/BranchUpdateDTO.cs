using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SODP.Domain.DTO
{
    public class BranchUpdateDTO
    {
        public int Id { get; set; }
        public string Sign { get; set; }
        public string Name { get; set; }
    }
}
