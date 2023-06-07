using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisProject.Areas.Identity.Data;
using SnackisProject.Gateways;
using SnackisProject.Models;
using System.Security.Claims;

namespace SnackisProject.Pages
{
    public class SubCategoryModel : PageModel
    {

        private readonly UserManager<SnackisUser> _userManager;
        private readonly SubCategoryGateway _subCategoryGateway;
        private readonly PostGateway _postGateway;


        [BindProperty]
        public Post Post { get; set; }

        [BindProperty]
        public string PostId { get; set; }
        public string UserId { get; set; }
        public List<SubCategory> SubCategories { get; set; }

        [BindProperty]
        public SubCategory SubCategory { get; set; }

        [BindProperty]
        public string SubcategoryId { get; set; }


        [BindProperty]
        public Category CategoryId { get; set; }

        [BindProperty]
        public Category Category { get; set; }
        public List<CustomPostModel> Posts { get; set; }
        public class CustomPostModel
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public DateTime CreatedAt { get; set; }
            public SnackisUser CreatedByUser { get; set; }
        }


        public SubCategoryModel(UserManager<SnackisUser> userManager, SubCategoryGateway subCategoryGateway, PostGateway postGateway)
        {
            _userManager = userManager;
            _subCategoryGateway = subCategoryGateway;
            _postGateway = postGateway;
            Posts = new List<CustomPostModel>();
        }
        public async Task<IActionResult> OnGetAsync(string subcategoryId, string deletePost)
        {
            var posts = await _postGateway.GetAllPostsBySubcategoryId(subcategoryId);

            if (deletePost != null)
            {
                await _postGateway.DeletePost(deletePost);
                return RedirectToPage("./Index");
            }
            SubCategory = await _subCategoryGateway.GetSubCategoryById(subcategoryId);
            UserId = _userManager.GetUserId(User);

            if (SubCategory == null)
            {
                return NotFound();
            }
            foreach (var post in posts)
            {
                var coustomPostModel = new CustomPostModel
                {
                    Id = post.Id,
                    CreatedByUser = await _userManager.FindByIdAsync(post.UserId),
                    Title = post.Title,
                    CreatedAt = post.CreatedAt
                };
                Posts.Add(coustomPostModel);
            }


            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            Guid guid = Guid.NewGuid();
            Post.Id = guid.ToString();
            UserId = _userManager.GetUserId(User);

            if (ModelState.IsValid || Post != null)
            {
                await _postGateway.CreatePost(Post);
                return RedirectToPage(new { subcategoryId = Post.SubCategoryId});
            }

            return BadRequest();
        }
    }
}
