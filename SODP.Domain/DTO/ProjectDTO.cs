using System;
using System.Collections.Generic;
using System.Text;

namespace SODP.Domain.DTO
{
    public class ProjectDTO
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string StageSign { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return Number.Trim() + StageSign.Trim() + "_" + Title.Trim();
        }
    }
}
