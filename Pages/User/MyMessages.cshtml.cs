using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisProject.Areas.Identity.Data;
using SnackisProject.Gateways;
using SnackisProject.Models;
using System.ComponentModel.DataAnnotations;

namespace SnackisProject.Pages.User
{
    [Authorize]
    public class MyMessagesModel : PageModel
    {

        private readonly UserManager<SnackisUser> _userManager;
        private readonly MessageGateway _messageGateway;

        [BindProperty]
        public Message ReplyMessage { get; set; }

        public List<CustomMessageModel> Messages { get; set; }

        public class CustomMessageModel
        {
            public string Id { get; set; }
            public SnackisUser FromUser { get; set; }
            public SnackisUser CurrentUser { get; set; }
            public string Text { get; set; }
            public DateTime SentAt { get; set; }
            public bool IsRead { get; set; }
        }

        public MyMessagesModel(UserManager<SnackisUser> userManager, MessageGateway messageGateway)
        {
            _userManager = userManager;
            _messageGateway = messageGateway;
            Messages = new List<CustomMessageModel>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return NotFound();

            var messages = await _messageGateway.GetAllMessagesByUserId(userId);
            foreach (var message in messages)
            {
                var customMessage = new CustomMessageModel
                {
                    Id = message.Id,
                    Text = message.Text,
                    SentAt = message.SentAt,
                    IsRead = message.IsRead,
                    FromUser = await _userManager.FindByIdAsync(message.FromUser),
                    CurrentUser = await _userManager.FindByIdAsync(message.ToUser)
                };
                Messages.Add(customMessage);
            }

            return Page();
        }


        public async Task<IActionResult> OnPostReplyMessage()
        {
            Guid guid = new Guid();
            ReplyMessage.Id = guid.ToString();
            ReplyMessage.SentAt = DateTime.Now;

            if (ModelState.IsValid || ReplyMessage != null)
            {
                 await _messageGateway.CreateMessage(ReplyMessage);
                return RedirectToPage(new { userId = ReplyMessage.FromUser});

            }

            return BadRequest();
        }
    }
}
