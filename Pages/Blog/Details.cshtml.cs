using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BlogApp.Models;
using BlogApp.Data;
using BlogApp.Helpers;

namespace BlogApp.Pages_Blog
{
    [Authorize(Roles = "Admin,Editor")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationBlogContext _context;

        public DetailsModel(ApplicationBlogContext context)
        {
            _context = context;
        }

        public Blog Blog { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog.Include(b => b.CreatedBy).FirstOrDefaultAsync(m => m.Id == id);

            if (blog == null)
            {
                return NotFound();
            }

            var userHelper = new UserHelper(User);
            var currentUserId = userHelper.GetCurrentUserId();

            if (currentUserId == null || (blog.CreatedById != currentUserId.Value && !userHelper.IsAdmin()))
            {
                return RedirectToPage("./AccessDenied");
            }

            Blog = blog;
            return Page();
        }

        public async Task<IActionResult> OnPostApproveAsync(Guid? id)
        {
            return await HandleStatusChangeAsync(id, BlogStatus.Approved);
        }

        public async Task<IActionResult> OnPostRejectAsync(Guid? id)
        {
            return await HandleStatusChangeAsync(id, BlogStatus.Rejected);
        }

        private async Task<IActionResult> HandleStatusChangeAsync(Guid? id, BlogStatus newStatus)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog.Include(b => b.CreatedBy).FirstOrDefaultAsync(m => m.Id == id);

            if (blog == null)
            {
                return NotFound();
            }

            var userHelper = new UserHelper(User);
            var currentUserId = userHelper.GetCurrentUserId();

            if (currentUserId == null || (blog.CreatedById != currentUserId.Value && !userHelper.IsAdmin()))
            {
                return RedirectToPage("./AccessDenied");
            }

            blog.Status = newStatus;
            _context.Attach(blog).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
