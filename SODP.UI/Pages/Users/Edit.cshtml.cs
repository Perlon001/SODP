using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SODP.Domain.DTO;
using SODP.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SODP.UI.Pages.Users
{
    // [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken()]
    public class EditModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;
        private readonly IRolesService _rolesService;

        public EditModel(IMapper mapper, IUsersService usersService, IRolesService rolesService)
        {
            _mapper = mapper;
            _usersService = usersService;
            _rolesService = rolesService;
        }

        [BindProperty]
        public UserDTO CurrentUser { get; set; }

        [BindProperty]
        public IDictionary<string,bool> AllRoles { get; set; }

        public string ReturnUrl { get; } = "/Users";

        public async Task<IActionResult> OnGet(int id)
        {
            var responseUsers = await _usersService.GetAsync(id);
            if (!responseUsers.Success)
            {
                return NotFound(responseUsers.Message);
            }
            CurrentUser = responseUsers.Data;

            AllRoles = _rolesService.GetAll().ToDictionary(x => x, x => false);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                CurrentUser.Roles = AllRoles.Where(x => x.Value).Select(x => x.Key).ToList();
                var response = await _usersService.UpdateAsync(_mapper.Map<UserUpdateDTO>(CurrentUser));

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
