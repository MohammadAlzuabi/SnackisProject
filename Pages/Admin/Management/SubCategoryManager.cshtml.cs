using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisProject.Gateways;
using SnackisProject.Models;

namespace SnackisProject.Pages.Admin.Management
{
    public class SubCategoryManagerModel : PageModel
    {

        private readonly CategoryGateway _categoryGateway;
        private readonly SubCategoryGateway _subCategoryGateway;

        public SubCategoryManagerModel(CategoryGateway categoryGateway , SubCategoryGateway subCategoryGateway)
        {
            _categoryGateway = categoryGateway;
            _subCategoryGateway = subCategoryGateway;
        }

        public List<SubCategory> SubCategories { get; set; }

        public List<Category> Categories { get; set; }

        [BindProperty]
        public SubCategory SubCategory { get; set; }

        [BindProperty]
        public Category Category { get; set; }

        [BindProperty]
        public string CategoryId { get; set; }
        public async Task<IActionResult> OnGetAsync(string categoryId, string deleteSubCategory)
        {
            SubCategories = await _subCategoryGateway.GetAllCategories();
            if (categoryId != null)
            {
                Category = await _categoryGateway.GetCategoryById(categoryId);
            }

            if (deleteSubCategory != null)
            {
                await _subCategoryGateway.DeleteSubCategory(deleteSubCategory);
                return RedirectToPage("./CategoryManager");

            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Guid guid = new Guid();
            SubCategory.Id = guid.ToString();


            if (ModelState.IsValid || SubCategory != null)
            {
                await _subCategoryGateway.CreateSubCategory(SubCategory);
            }

            return RedirectToPage(new { categoryId = SubCategory.CategoryId});
        }
    }
}
