using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SnackisProject.Areas.Identity.Data;

namespace SnackisProject.Pages.Admin.Management
{
    public class ManageUserRolesModel : PageModel
    {
        private readonly UserManager<SnackisUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public string ErrorMessage { get; set; }
        public string Username { get; set; }

        [BindProperty]
        public List<ManageUserWithRoles> ManageUserWithRolesList { get; set; }

        public class ManageUserWithRoles
        {
            public string RoleId { get; set; }
            public string RoleName { get; set; }
            public bool Selected { get; set; }
        }

        public ManageUserRolesModel(UserManager<SnackisUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            ManageUserWithRolesList = new List<ManageUserWithRoles>();
        }

        public async Task<IActionResult> OnGet(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ErrorMessage = $"User with Id = {userId} cannot be found";
                return NotFound(ErrorMessage);
            }

            Username = user.UserName;

            var roles = await _roleManager.Roles.ToListAsync();
            foreach (var role in roles)
            {
                var manageUserWithRoles = new ManageUserWithRoles
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    manageUserWithRoles.Selected = true;
                }

                ManageUserWithRolesList.Add(manageUserWithRoles);
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Page();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return Page();
            }

            result = await _userManager.AddToRolesAsync(user, ManageUserWithRolesList
                .Where(r => r.Selected)
                .Select(r => r.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return Page();
            }

            return RedirectToPage("UserRolesManager");
        }
    }
}
