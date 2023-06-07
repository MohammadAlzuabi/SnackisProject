using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisProject.Areas.Identity.Data;
using SnackisProject.Gateways;
using SnackisProject.Models;

namespace SnackisProject.Pages
{
    public class IndexModel : PageModel
    {
        private UserManager<SnackisUser> _userManager;
        
        private readonly CategoryGateway _categoryGateway;
        private readonly SubCategoryGateway _subCategoryGateway;
        public SnackisUser GetUser { get; set; }

        public List<Category> Categories { get; set; }
        public List<SubCategory> SubCategories { get; set; }

        public IndexModel(UserManager<SnackisUser> userManager, CategoryGateway categoryGateway, SubCategoryGateway subCategoryGateway)
        {
            _userManager = userManager;
            _categoryGateway = categoryGateway;
            _subCategoryGateway = subCategoryGateway;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            GetUser = await _userManager.GetUserAsync(User);
            Categories = await _categoryGateway.GetAllCategories();
            SubCategories = await _subCategoryGateway.GetAllCategories();

            return Page();
        }
    }
}