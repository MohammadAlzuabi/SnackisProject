using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SnackisProject.Areas.Identity.Data;

namespace SnackisProject.Pages.Admin
{
    public class RoleManagerModel : PageModel
    {
        public readonly UserManager<SnackisUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public List<SnackisUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }

        [BindProperty]
        public string RoleName { get; set; }


        [BindProperty(SupportsGet = true)]
        public string AddUserId { get; set; } // Lägg till


        [BindProperty(SupportsGet = true)]
        public string RemoveUserId { get; set; } // Ta bort


        [BindProperty(SupportsGet = true)]
        public string Role { get; set; }

        // Demo av roller
        public bool IsUser { get; set; }

        public bool IsAdmin { get; set; }
        public RoleManagerModel(RoleManager<IdentityRole> roleManager, UserManager<SnackisUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Roles = await _roleManager.Roles.ToListAsync();
            Users = await _userManager.Users.ToListAsync();


            if (AddUserId != null)
            {
                var alterUser = await _userManager.FindByIdAsync(AddUserId); // Letar efter personen vi vill lägga rol till
                var roleResult = await _userManager.AddToRoleAsync(alterUser, Role); // Ta vi personen och ge honom rollen 
            }
            if (RemoveUserId != null)
            {
                var alterUser = await _userManager.FindByIdAsync(RemoveUserId);
                var roleResult = await _userManager.RemoveFromRoleAsync(alterUser, Role);
            }

            //Demo av roller

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                IsUser = await _userManager.IsInRoleAsync(currentUser, "User");
                IsAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            }



            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (RoleName != null)
            {
                await CreateRole(RoleName);
            }

            return RedirectToPage("./RoleManager");
        }

        public async Task CreateRole(string roles)
        {
            bool exist = await _roleManager.RoleExistsAsync(roles);

            if (!exist)
            {
                var role = new IdentityRole
                {
                    Name = RoleName,

                };
                await _roleManager.CreateAsync(role);
            }
        }

    }
}
