using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SnackisProject.Areas.Identity.Data;

namespace SnackisProject.Pages.Admin.Management
{
    public class UserRolesManagerModel : PageModel
    {
        private readonly UserManager<SnackisUser> _userManager;

        [BindProperty]
        public string DeleteUserId { get; set; }

        public List<UserWithRoles> UserRolesList { get; set; }

        public class UserWithRoles
        {
            public string UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public IEnumerable<string> Roles { get; set; }
        }

        public UserRolesManagerModel(UserManager<SnackisUser> userManager)
        {
            _userManager = userManager;
            UserRolesList = new List<UserWithRoles>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userRoles = new UserWithRoles
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName,
                    Roles = await GetUserRoles(user)
                };
                UserRolesList.Add(userRoles);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (DeleteUserId != null)
            {
                var user = await _userManager.FindByIdAsync(DeleteUserId);
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToPage();
                }
            }

            return NotFound();
        }

        private async Task<List<string>> GetUserRoles(SnackisUser user)
        {
            return new(await _userManager.GetRolesAsync(user));
        }
    }
}
