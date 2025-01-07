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
    public class IndexModel : PageModel
    {
        private readonly ApplicationBlogContext _context;

        public IndexModel(ApplicationBlogContext context)
        {
            _context = context;
        }

        public IList<Blog> Blog { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Blog = await _context.Blog.ToListAsync();
        }
    }
}
