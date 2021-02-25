using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SODP.Domain.DTO
{
    public class StageDTO
    {
        public int Id { get; set; }

        [MinLength(2)]
        public string Sign { get; set; }

        public string Title { get; set; }
    }
}
