using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisProject.Gateways;
using SnackisProject.Models;

namespace SnackisProject.Pages.Admin.Management
{
    public class ManageCategoryModel : PageModel
    {
        private readonly CategoryGateway _categoryGateway;
        public List<Category> Categories { get; set; }

        [BindProperty]
        public Category EditCategory { get; set; }


        public ManageCategoryModel(CategoryGateway categoryGateway)
        {
            _categoryGateway = categoryGateway;
        }

        public async Task<IActionResult> OnGetAsync(string deleteCategory, string editCategory)
        {
            Categories = await _categoryGateway.GetAllCategories();
            if (deleteCategory != null)
            {
                await _categoryGateway.DeleteCategory(deleteCategory);
                return RedirectToPage();
            }
            if(editCategory != null)
            {
                EditCategory =  await _categoryGateway.GetCategoryById(editCategory);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid || EditCategory != null)
            {
                await _categoryGateway.EditCategory(EditCategory);
                return RedirectToPage();
            }
            //Subjects = await SubjectManager.GettAllSubjects();
            return BadRequest();
        }
    }
}
