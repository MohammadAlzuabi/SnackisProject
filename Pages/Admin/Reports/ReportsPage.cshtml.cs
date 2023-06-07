using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisProject.Areas.Identity.Data;
using SnackisProject.Gateways;
using SnackisProject.Models;
using System.Composition;

namespace SnackisProject.Pages.Admin.Reports
{
    public class ReportsPageModel : PageModel
    {
        private readonly PostGateway _postGateway;
        private readonly ReportGateway _reportGateway;
        private readonly CommentGateway _commentGateway;
        private readonly UserManager<SnackisUser> _userManager;

        public List<CustomReportModel> Reports { get; set; }

        [BindProperty]
        public string ReportId { get; set; }
        public class CustomReportModel
        {
            public string Id { get; set; }
            public Post Post { get; set; }
            public string Content { get; set; }
            public DateTime CreatedAt { get; set; }
            public SnackisUser ByUser { get; set; }
        }

        public ReportsPageModel(ReportGateway reportGateway, UserManager<SnackisUser> userManager, PostGateway postGateway, CommentGateway commentGateway)
        {
            _userManager = userManager;
            _postGateway = postGateway;
            _reportGateway = reportGateway;
            _commentGateway = commentGateway;
            Reports = new List<CustomReportModel>();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var reports = await _reportGateway.GetAllReports();

            foreach (var report in reports)
            {
                if (report.PostId != null)
                {
                    var post = await _postGateway.GetPostById(report.PostId);


                    var customReport = new CustomReportModel
                    {
                        Id = report.Id,
                        Content = report.Text,
                        CreatedAt = report.CreatedAt,
                        ByUser = await _userManager.FindByIdAsync(report.ByUser)
                    };

                    if (post != null)
                    {
                        customReport.Post = post;
                        Reports.Add(customReport);
                    }
                    else
                    {
                        await _reportGateway.DeleteReport(report.Id);
                        break;
                    }
                }
                else
                {
                    await _reportGateway.DeleteReport(report.Id);
                }
            }
            return Page();
        }
    }
}
