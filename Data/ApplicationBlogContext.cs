using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogApp.Models;

namespace BlogApp.Data
{
    public class ApplicationBlogContext : DbContext
    {
        public ApplicationBlogContext (DbContextOptions<ApplicationBlogContext> options)
            : base(options)
        {
        }

        public DbSet<BlogApp.Models.Blog> Blog { get; set; } = default!;
        public DbSet<BlogApp.Models.User> User { get; set; } = default!;
    }
}
