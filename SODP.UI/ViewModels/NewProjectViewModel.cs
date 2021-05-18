using Microsoft.AspNetCore.Mvc.Rendering;
using SODP.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SODP.UI.ViewModels
{
    public class NewProjectViewModel
    {
        public ProjectDTO Project { get; set; }
        public IEnumerable<SelectListItem> Stages { get; set; }
    }
}
