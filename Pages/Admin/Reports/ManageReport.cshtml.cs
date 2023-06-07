using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisProject.Areas.Identity.Data;
using SnackisProject.Gateways;
using SnackisProject.Models;

namespace SnackisProject.Pages.Admin.Reports
{
    public class ManageReportModel : PageModel
    {
        private readonly PostGateway _postGateway;
        private readonly ReportGateway _reportGateway;
        private readonly CommentGateway _commentGateway;
        private readonly UserManager<SnackisUser> _userManager;



        public CustomReportModel Report { get; set; }

        public class CustomReportModel
        {
            public string Id { get; set; }
            public SnackisUser ByUser { get; set; }
            public Post Post { get; set; }
            public string Text { get; set; }
            public DateTime CreatedAt { get; set; }
        }
        public ManageReportModel(ReportGateway reportGateway, PostGateway postGateway, UserManager<SnackisUser> userManager)
        {
            _userManager = userManager;
            _postGateway = postGateway;
            _reportGateway = reportGateway;
        }
        public async Task<IActionResult> OnGetAsync(string reportId, string deleteReport, string deletePost)
        {
            if (deleteReport != null)
            {
                await _reportGateway.DeleteReport(deleteReport);
                return RedirectToPage("ReportsPage");
            }
            if (deletePost != null)
            {
                await _postGateway.DeletePost(deletePost);
                return RedirectToPage("ReportsPage");
            }
            var report = await _reportGateway.GetReportById(reportId);

            if (report.PostId != null)
            {
                Report = new CustomReportModel
                {
                    Id = report.Id,
                    Text = report.Text,
                    CreatedAt = report.CreatedAt,
                    Post = await _postGateway.GetPostById(report.PostId),
                    ByUser = await _userManager.FindByIdAsync(report.ByUser)
                };
            }

            return Page();
        }

        //public async Task<IActionResult> OnPost()
        //{
        //    if (DeleteReportId != null)
        //    {
        //        await _reportGateway.DeleteReport(DeleteReportId);
        //        return RedirectToPage("ReportsPage");

        //    }

        //    return RedirectToPage(new { reportId = DeleteReportId });
        //}


    }
}
