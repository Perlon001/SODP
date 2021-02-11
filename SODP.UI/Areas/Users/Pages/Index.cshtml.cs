using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SODP.Domain.Services;
using SODP.Model;

namespace SODP.UI.Areas.Users.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUsersService _usersService;

        public IndexModel(IUsersService usersService)
        {
            _usersService = usersService;
        }
        public ServicePageResponse<User> Users { get; set; }

        public async Task OnGet()
        {
            Users = await _usersService.GetAllAsync();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var response = await _usersService.DeleteAsync(id);
            if (!response.Success)
            {
                Users = await _usersService.GetAllAsync();
                return Page();
            }
            return RedirectToPage("Index");
        }
    }
}
