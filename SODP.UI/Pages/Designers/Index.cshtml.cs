using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SODP.Domain.DTO;

namespace SODP.UI.Pages.Designers
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }

        public PartialViewResult OnGetDesignerDetails()
        {
            var partialViewResult = new PartialViewResult
            {
                ViewName = "_DesignerPartialView",
                ViewData = new ViewDataDictionary<DesignerDTO>(ViewData, new DesignerDTO())
            };

            return partialViewResult;
        }

        public void OnPostDesignerDetails()
        {

        }
    }
}
