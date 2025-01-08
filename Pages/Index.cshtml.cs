using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using BlogApp.Models;
using BlogApp.Data;

namespace BlogApp.Pages;

public class IndexModel : PageModel
{
    private readonly ApplicationBlogContext _context;

    public IndexModel(ApplicationBlogContext context)
    {
        _context = context;
    }
    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Authenticate user from database
        var user = _context.User
        .Where(u => u.Email == Email && u.Password == Password)   
        .Select(u => new { u.Id, u.Email, u.Password, u.Role })
        .FirstOrDefault();
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        // Create claims for user
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

        return RedirectToPage("/User/Index");
    }
}

