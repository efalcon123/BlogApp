
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlogApp.Pages_User
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationBlogContext _context;

        public CreateModel(ApplicationBlogContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Hash the user's password using BCrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(User.Password);

            // Set the hashed password back to the User model
            User.Password = hashedPassword;

            // Add the user to the database
            _context.User.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
