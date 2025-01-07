using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BlogApp.Data;
using BlogApp.Models;

namespace BlogApp.Pages_User
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly BlogApp.Data.ApplicationBlogContext _context;

        public IndexModel(BlogApp.Data.ApplicationBlogContext context)
        {
            _context = context;
        }

        public IList<User> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            User = await _context.User.ToListAsync();
        }
    }
}
