using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogApp.Models;
using BlogApp.Data;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlogApp.Pages_Blog
{
    [Authorize(Roles = "Admin,Editor")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationBlogContext _context;

        public CreateModel(ApplicationBlogContext context)
        {
            _context = context;
        }

        public Guid CurrentUserId { get; private set; }

        [BindProperty]
        public Blog Blog { get; set; } = default!;

        [BindProperty]
        public IFormFile? BannerFile { get; set; }

        public IActionResult OnGet()
        {
            CurrentUserId = GetCurrentUserId();
            if (CurrentUserId == Guid.Empty)
            {
                return RedirectToPage("/Error");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CurrentUserId = GetCurrentUserId();
            if (CurrentUserId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "Unable to determine the current user.");
                return Page();
            }

            var user = await GetUserByIdAsync(CurrentUserId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to find the user for the current ID.");
                return Page();
            }

            Blog.CreatedById = CurrentUserId;
            Blog.CreatedBy = user;

            if (!ModelState.IsValid)
            {
                LogModelErrors();
                return Page();
            }

            Blog.Content = SanitizeContent(Blog.Content);

            if (BannerFile != null && !await TryProcessBannerFileAsync(BannerFile))
            {
                return Page();
            }

            _context.Blog.Add(Blog);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        #region Helper Methods
        private Guid GetCurrentUserId()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : Guid.Empty;
        }


        private async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.User.FindAsync(userId);
        }


        private void LogModelErrors()
        {
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                }
            }
        }

        private string SanitizeContent(string? content)
        {
            var sanitizer = new HtmlSanitizer();
            return sanitizer.Sanitize(content ?? string.Empty);
        }
        private async Task<bool> TryProcessBannerFileAsync(IFormFile bannerFile)
        {
            if (bannerFile.Length > 5 * 1024 * 1024) // 5 MB limit
            {
                ModelState.AddModelError("BannerFile", "File size cannot exceed 5MB.");
                return false;
            }

            using var memoryStream = new MemoryStream();
            await bannerFile.CopyToAsync(memoryStream);
            Blog.Banner = memoryStream.ToArray();

            return true;
        }

        #endregion
    }
}
