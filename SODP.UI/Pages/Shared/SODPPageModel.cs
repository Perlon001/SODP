using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SODP.UI.Pages.Shared
{
    public abstract class SODPPageModel : PageModel
    {
        public string ReturnUrl { get; protected set; }
    }
}
