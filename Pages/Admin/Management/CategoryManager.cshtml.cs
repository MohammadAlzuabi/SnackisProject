using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisProject.Gateways;
using SnackisProject.Models;

namespace SnackisProject.Pages.Admin.Management
{
    public class CategoryManagerModel : PageModel
    {
        private readonly CategoryGateway _categoryGateway;
        public List<Category> Categories { get; set; }

        [BindProperty]
        public Category Category { get; set; }


        public CategoryManagerModel(CategoryGateway categoryGateway)
        {
            _categoryGateway = categoryGateway;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Categories = await _categoryGateway.GetAllCategories();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Guid guid = new Guid();
            Category.Id = guid.ToString();
            if (ModelState.IsValid)
            {
                await _categoryGateway.CreateCategory(Category);
                return RedirectToPage();
            }
            //Subjects = await SubjectManager.GettAllSubjects();
            return BadRequest();
        }

    }
}
