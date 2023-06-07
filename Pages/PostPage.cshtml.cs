using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisProject.Areas.Identity.Data;
using SnackisProject.Gateways;
using SnackisProject.Models;

namespace SnackisProject.Pages
{
    public class PostPageModel : PageModel
    {
        private readonly PostGateway _postGateway;
        private readonly CommentGateway _commentGateway;
        private readonly ReportGateway _reportGateway;
        private readonly MessageGateway _messageGateway;
        private readonly SubCategoryGateway _subCategoryGateway;
        private readonly UserManager<SnackisUser> _userManager;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public Message SendMessage { get; set; }


        [BindProperty]
        public Comment Comment { get; set; }


        [BindProperty]
        public Report Report { get; set; }
        [BindProperty]
        public Post Post { get; set; }

        public List<CustomPostModel> Posts { get; set; }
        public string UserId { get; set; }
        public SubCategory SubCategory { get; set; }

        public SnackisUser CreatedByUser { get; set; }

        public List<CustomCommentModel> Comments { get; set; }

        public class CustomCommentModel
        {
            public string Id { get; set; }
            public string Text { get; set; }
            public DateTime CreatedAt { get; set; }
            public SnackisUser CreatedByUser { get; set; }
        }

        public class CustomPostModel
        {
            public string Id { get; set; }
            
            public string Title { get; set; }
            public string Text { get; set; }
            public DateTime CreatedAt { get; set; }
            public SnackisUser CreatedByUser { get; set; }
        }


        public PostPageModel(PostGateway postGateway, CommentGateway commentGateway, SubCategoryGateway subCategoryGateway, UserManager<SnackisUser> userManager, ReportGateway reportGateway, MessageGateway messageGateway)
        {
            _postGateway = postGateway;
            _commentGateway = commentGateway;
            _subCategoryGateway = subCategoryGateway;
            _userManager = userManager;
            Comments = new List<CustomCommentModel>();
            _reportGateway = reportGateway;
            _messageGateway = messageGateway;
        }

        public async Task<IActionResult> OnGetAsync(string postId, string deleteComment)
        {
            Post = await _postGateway.GetPostById(postId);
            if (Post == null)
            {
                return NotFound();
            }
            SubCategory = await _subCategoryGateway.GetSubCategoryById(Post.SubCategoryId);
            UserId = _userManager.GetUserId(User);
            CreatedByUser = await _userManager.FindByIdAsync(Post.UserId);
            var comments = await _commentGateway.GetCommentByPostId(postId);
            foreach (var comment in comments)
            {
                var customComment = new CustomCommentModel
                {
                    Id = comment.Id,
                    Text = comment.Text,
                    CreatedAt = comment.CreatedAt,
                    CreatedByUser = await _userManager.FindByIdAsync(comment.UserId)
                };
                Comments.Add(customComment);
            }


            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            Guid guid = Guid.NewGuid();
            Comment.Id = guid.ToString();
            UserId = _userManager.GetUserId(User);
            if (ModelState.IsValid || Comment != null)
            {
                await _commentGateway.CreateComment(Comment);
                return RedirectToPage(new {postId = Comment.PostId});
            }
            return Page();
        }

        public async Task<IActionResult> OnPostCreateReport()
        {
            Guid guid = Guid.NewGuid();
            Report.Id = guid.ToString();
            if (Report.ByUser != null && Report.PostId != null)
            {
                await _reportGateway.CreateReport(Report);
                return RedirectToPage(new { postId = Report.PostId });
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSendMessage()
        {
            Guid guid = Guid.NewGuid();
            SendMessage.Id = guid.ToString();
            SendMessage.SentAt = DateTime.Now;
            if (SendMessage.FromUser != null && SendMessage.ToUser != null && (SendMessage.Text != null || SendMessage.Text != string.Empty))
            {
                await _messageGateway.CreateMessage(SendMessage);
               return RedirectToPage("./Index");

            }
            StatusMessage = "Failed to send message!";
            return Page();
        }

    }
}
