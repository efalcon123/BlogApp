using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models;

public class User
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, ErrorMessage = "Password must be between 8 and 100 characters.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    [RegularExpression("Admin|Editor", ErrorMessage = "Role must be either Admin, Editor, or User.")]
   
    public string? Role { get; set; }
}