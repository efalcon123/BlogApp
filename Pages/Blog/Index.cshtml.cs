using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BlogApp.Models;
using BlogApp.Data;
using BlogApp.Helpers;

namespace BlogApp.Pages_Blog
{
    [Authorize(Roles = "Admin,Editor")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationBlogContext _context;

        public IndexModel(ApplicationBlogContext context)
        {
            _context = context;
        }

        public IList<Blog> Blog { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var userHelper = new UserHelper(User);        
            var userId = userHelper.GetCurrentUserId();
            if (userHelper.IsAdmin())
            {
                Blog = await _context.Blog.Include(b => b.CreatedBy).ToListAsync();
            }
            else
            {
                Blog = await _context.Blog
                    .Where(b => b.CreatedById == userId.Value)
                    .Include(b => b.CreatedBy)
                    .ToListAsync();
            }
        }
    }
}
