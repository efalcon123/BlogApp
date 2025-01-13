using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BlogApp.Models;
using BlogApp.Data;
using BlogApp.Helpers;

namespace BlogApp.Pages_Blog
{
    [Authorize(Roles = "Admin,Editor")]
    public class EditModel : PageModel
    {
        private readonly ApplicationBlogContext _context;

        public EditModel(ApplicationBlogContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Blog Blog { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToPage("./AccessDenied");
            }

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

        public async Task<IActionResult> OnPostAsync()
        {
            var userHelper = new UserHelper(User);
            var currentUserId = userHelper.GetCurrentUserId();

            if (currentUserId == null || (Blog.CreatedById != currentUserId.Value && !userHelper.IsAdmin()))
            {
                return RedirectToPage("./AccessDenied");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Blog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(Blog.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BlogExists(Guid id)
        {
            return _context.Blog.Any(e => e.Id == id);
        }
    }
}
