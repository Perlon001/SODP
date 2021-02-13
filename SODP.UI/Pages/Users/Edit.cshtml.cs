using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SODP.Domain.Services;
using SODP.Model;

namespace SODP.UI.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly IUsersService _usersService;

        public EditModel(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [BindProperty]
        public User CurrentUser { get; set; }

        public string ReturnUrl { get; } = "/Users";

        public async Task<IActionResult> OnGet(int id)
        {
            var response = await _usersService.GetAsync(id);
            if (response == null)
            {
                return NotFound();
            }
            CurrentUser = response.Data;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var response = await _usersService.UpdateAsync(CurrentUser);

                if (!response.Success)
                {
                    return Page();
                }
                return RedirectToPage("Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
