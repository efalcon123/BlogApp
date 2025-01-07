using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BlogApp.Models;
using BlogApp.Data;

namespace BlogApp.Pages_Blog
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationBlogContext _context;

        public DetailsModel(ApplicationBlogContext context)
        {
            _context = context;
        }

        public Blog Blog { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog.FirstOrDefaultAsync(m => m.Id == id);

            if (blog is not null)
            {
                Blog = blog;

                return Page();
            }

            return NotFound();
        }
    }
}
