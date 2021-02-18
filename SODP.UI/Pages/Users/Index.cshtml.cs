using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SODP.Domain.DTO;
using SODP.Domain.Services;
using SODP.UI.Pages.Shared;

namespace SODP.UI.Pages.Users
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : SODPPageModel
    {
        private readonly IUsersService _usersService;

        public IndexModel(IUsersService usersService)
        {
            _usersService = usersService;
            ReturnUrl = "/Users";
        }
        public IEnumerable<UserDTO> Users { get; set; }

        public async Task OnGet()
        {
            var serviceResponse = await _usersService.GetAllAsync();
            Users = serviceResponse.Data.Collection;
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var response = await _usersService.DeleteAsync(id);
            if (!response.Success)
            {
                var serviceResponse = await _usersService.GetAllAsync();
                Users = serviceResponse.Data.Collection;
                return Page();
            }
            return RedirectToPage("Index");
        }
    }
}
